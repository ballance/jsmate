using JsMate.Domain.Enums;
using JsMate.Domain.ValueObjects;

namespace JsMate.Domain.Entities;

/// <summary>
/// Base class for all chess pieces.
/// </summary>
public abstract class ChessPiece
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public PieceTeam Team { get; init; }
    public abstract PieceType Type { get; }
    public BoardPosition Position { get; private set; }
    public bool IsActive { get; private set; } = true;
    public int PieceNumber { get; init; }

    protected ChessPiece(PieceTeam team, BoardPosition position, int pieceNumber = 1)
    {
        Team = team;
        Position = position;
        PieceNumber = pieceNumber;
    }

    // Parameterless constructor for serialization
    protected ChessPiece() { }

    /// <summary>
    /// Gets all valid moves for this piece on the given board.
    /// </summary>
    public abstract IReadOnlyList<BoardPosition> GetValidMoves(IChessBoard board);

    /// <summary>
    /// Gets all positions this piece can attack (may differ from valid moves for pawns).
    /// </summary>
    public virtual IReadOnlyList<BoardPosition> GetAttackPositions(IChessBoard board) => GetValidMoves(board);

    /// <summary>
    /// Moves the piece to a new position.
    /// </summary>
    public void MoveTo(BoardPosition newPosition)
    {
        if (!IsActive)
            throw new InvalidOperationException("Cannot move a captured piece.");

        Position = newPosition;
    }

    /// <summary>
    /// Marks this piece as captured.
    /// </summary>
    public void Capture()
    {
        IsActive = false;
    }

    /// <summary>
    /// Restores this piece to active state (used for move simulation).
    /// </summary>
    public void Restore()
    {
        IsActive = true;
    }

    /// <summary>
    /// Gets the direction multiplier for this piece's team.
    /// White pieces move "up" (decreasing row), Black pieces move "down" (increasing row).
    /// </summary>
    protected int GetForwardDirection() => Team == PieceTeam.White ? -1 : 1;

    /// <summary>
    /// Checks if a position is occupied by a piece of the same team.
    /// </summary>
    protected bool IsBlockedByOwnPiece(IChessBoard board, BoardPosition position)
    {
        var piece = board.GetPieceAt(position);
        return piece != null && piece.Team == Team;
    }

    /// <summary>
    /// Checks if a position is occupied by an enemy piece.
    /// </summary>
    protected bool IsOccupiedByEnemy(IChessBoard board, BoardPosition position)
    {
        var piece = board.GetPieceAt(position);
        return piece != null && piece.Team != Team;
    }

    /// <summary>
    /// Adds sliding moves in a given direction until blocked.
    /// </summary>
    protected void AddSlidingMoves(
        IChessBoard board,
        List<BoardPosition> moves,
        int rowDelta,
        int colDelta)
    {
        var currentRow = Position.Row + rowDelta;
        var currentCol = Position.Col + colDelta;

        while (BoardPosition.IsValidCoordinate(currentRow) && BoardPosition.IsValidCoordinate(currentCol))
        {
            var pos = new BoardPosition(currentRow, currentCol);

            if (IsBlockedByOwnPiece(board, pos))
                break;

            moves.Add(pos);

            if (IsOccupiedByEnemy(board, pos))
                break;

            currentRow += rowDelta;
            currentCol += colDelta;
        }
    }

    public override string ToString() => $"{Team} {Type} at {Position}";
}
