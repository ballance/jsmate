using JsMate.Domain.Enums;
using JsMate.Domain.ValueObjects;

namespace JsMate.Domain.Entities.Pieces;

public sealed class Pawn : ChessPiece
{
    public override PieceType Type => PieceType.Pawn;
    public bool HasMoved { get; private set; }

    public Pawn(PieceTeam team, BoardPosition position, int pieceNumber = 1)
        : base(team, position, pieceNumber)
    {
    }

    // For serialization
    public Pawn() { }

    public override IReadOnlyList<BoardPosition> GetValidMoves(IChessBoard board)
    {
        var moves = new List<BoardPosition>();
        var direction = GetForwardDirection();
        var startRow = Team == PieceTeam.White ? 6 : 1;

        // Single square forward
        var oneForward = BoardPosition.TryCreate(Position.Row + direction, Position.Col);
        if (oneForward.HasValue && board.IsEmpty(oneForward.Value))
        {
            moves.Add(oneForward.Value);

            // Double square forward from starting position
            if (Position.Row == startRow)
            {
                var twoForward = BoardPosition.TryCreate(Position.Row + (2 * direction), Position.Col);
                if (twoForward.HasValue && board.IsEmpty(twoForward.Value))
                {
                    moves.Add(twoForward.Value);
                }
            }
        }

        // Diagonal captures
        AddDiagonalCapture(board, moves, direction, -1); // Left diagonal
        AddDiagonalCapture(board, moves, direction, 1);  // Right diagonal

        // En passant
        AddEnPassantMoves(board, moves, direction);

        return moves;
    }

    public override IReadOnlyList<BoardPosition> GetAttackPositions(IChessBoard board)
    {
        var attacks = new List<BoardPosition>();
        var direction = GetForwardDirection();

        var leftDiag = BoardPosition.TryCreate(Position.Row + direction, Position.Col - 1);
        if (leftDiag.HasValue)
            attacks.Add(leftDiag.Value);

        var rightDiag = BoardPosition.TryCreate(Position.Row + direction, Position.Col + 1);
        if (rightDiag.HasValue)
            attacks.Add(rightDiag.Value);

        return attacks;
    }

    private void AddDiagonalCapture(IChessBoard board, List<BoardPosition> moves, int rowDirection, int colDirection)
    {
        var diagonal = BoardPosition.TryCreate(Position.Row + rowDirection, Position.Col + colDirection);
        if (diagonal.HasValue && IsOccupiedByEnemy(board, diagonal.Value))
        {
            moves.Add(diagonal.Value);
        }
    }

    private void AddEnPassantMoves(IChessBoard board, List<BoardPosition> moves, int direction)
    {
        var enPassantTarget = board.EnPassantTarget;
        if (!enPassantTarget.HasValue)
            return;

        // En passant is only valid from the correct rank (4 for white, 3 for black)
        var enPassantRank = Team == PieceTeam.White ? 3 : 4;
        if (Position.Row != enPassantRank)
            return;

        // Check if the en passant target is diagonally adjacent
        var targetCol = enPassantTarget.Value.Col;
        if (Math.Abs(Position.Col - targetCol) == 1)
        {
            var targetRow = Position.Row + direction;
            if (enPassantTarget.Value.Row == targetRow)
            {
                moves.Add(enPassantTarget.Value);
            }
        }
    }

    /// <summary>
    /// Checks if this pawn can be promoted (reached the opposite end).
    /// </summary>
    public bool CanBePromoted()
    {
        var promotionRank = Team == PieceTeam.White ? 0 : 7;
        return Position.Row == promotionRank;
    }

    public new void MoveTo(BoardPosition newPosition)
    {
        base.MoveTo(newPosition);
        HasMoved = true;
    }

    /// <summary>
    /// Resets the HasMoved flag (used for undo operations).
    /// </summary>
    public void ResetHasMoved(bool hasMoved)
    {
        HasMoved = hasMoved;
    }
}
