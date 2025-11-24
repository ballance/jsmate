using JsMate.Domain.Enums;
using JsMate.Domain.ValueObjects;

namespace JsMate.Domain.Entities.Pieces;

public sealed class King : ChessPiece
{
    public override PieceType Type => PieceType.King;
    public bool HasMoved { get; private set; }

    private static readonly (int Row, int Col)[] AllDirections =
    [
        (-1, 0),  // North
        (-1, 1),  // NE
        (0, 1),   // East
        (1, 1),   // SE
        (1, 0),   // South
        (1, -1),  // SW
        (0, -1),  // West
        (-1, -1)  // NW
    ];

    public King(PieceTeam team, BoardPosition position, int pieceNumber = 1)
        : base(team, position, pieceNumber)
    {
    }

    // For serialization
    public King() { }

    public override IReadOnlyList<BoardPosition> GetValidMoves(IChessBoard board)
    {
        var moves = new List<BoardPosition>();

        // Standard king moves (one square in any direction)
        foreach (var (rowDelta, colDelta) in AllDirections)
        {
            var newPos = BoardPosition.TryCreate(Position.Row + rowDelta, Position.Col + colDelta);
            if (newPos.HasValue && !IsBlockedByOwnPiece(board, newPos.Value))
            {
                moves.Add(newPos.Value);
            }
        }

        // Castling
        if (!HasMoved && !board.IsInCheck(Team))
        {
            AddCastlingMoves(board, moves);
        }

        return moves;
    }

    private void AddCastlingMoves(IChessBoard board, List<BoardPosition> moves)
    {
        var backRank = Team == PieceTeam.White ? 7 : 0;

        // King should be on its starting position
        if (Position.Row != backRank || Position.Col != 4)
            return;

        // Kingside castling (O-O)
        if (CanCastleKingside(board, backRank))
        {
            moves.Add(new BoardPosition(backRank, 6));
        }

        // Queenside castling (O-O-O)
        if (CanCastleQueenside(board, backRank))
        {
            moves.Add(new BoardPosition(backRank, 2));
        }
    }

    private bool CanCastleKingside(IChessBoard board, int backRank)
    {
        // Check if kingside rook exists and hasn't moved
        var rookPos = new BoardPosition(backRank, 7);
        var rook = board.GetPieceAt(rookPos);
        if (rook is not Rook { HasMoved: false } || rook.Team != Team)
            return false;

        // Check if squares between king and rook are empty
        if (!board.IsEmpty(new BoardPosition(backRank, 5)) ||
            !board.IsEmpty(new BoardPosition(backRank, 6)))
            return false;

        // Check if king doesn't pass through or end up in check
        // (Simplified - full implementation would simulate the moves)
        return !IsSquareUnderAttack(board, new BoardPosition(backRank, 5)) &&
               !IsSquareUnderAttack(board, new BoardPosition(backRank, 6));
    }

    private bool CanCastleQueenside(IChessBoard board, int backRank)
    {
        // Check if queenside rook exists and hasn't moved
        var rookPos = new BoardPosition(backRank, 0);
        var rook = board.GetPieceAt(rookPos);
        if (rook is not Rook { HasMoved: false } || rook.Team != Team)
            return false;

        // Check if squares between king and rook are empty
        if (!board.IsEmpty(new BoardPosition(backRank, 1)) ||
            !board.IsEmpty(new BoardPosition(backRank, 2)) ||
            !board.IsEmpty(new BoardPosition(backRank, 3)))
            return false;

        // Check if king doesn't pass through or end up in check
        return !IsSquareUnderAttack(board, new BoardPosition(backRank, 2)) &&
               !IsSquareUnderAttack(board, new BoardPosition(backRank, 3));
    }

    private bool IsSquareUnderAttack(IChessBoard board, BoardPosition position)
    {
        var enemyTeam = Team == PieceTeam.White ? PieceTeam.Black : PieceTeam.White;
        var enemyPieces = board.GetActivePieces(enemyTeam);

        return enemyPieces.Any(p => p.GetAttackPositions(board).Contains(position));
    }

    public override IReadOnlyList<BoardPosition> GetAttackPositions(IChessBoard board)
    {
        // King's attack positions are just the 8 adjacent squares
        // This must NOT call GetValidMoves to avoid infinite recursion when checking for check
        var attacks = new List<BoardPosition>();

        foreach (var (rowDelta, colDelta) in AllDirections)
        {
            var newPos = BoardPosition.TryCreate(Position.Row + rowDelta, Position.Col + colDelta);
            if (newPos.HasValue)
            {
                attacks.Add(newPos.Value);
            }
        }

        return attacks;
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
