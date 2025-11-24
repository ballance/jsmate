using JsMate.Domain.Enums;
using JsMate.Domain.ValueObjects;

namespace JsMate.Domain.Entities.Pieces;

public sealed class Rook : ChessPiece
{
    public override PieceType Type => PieceType.Rook;
    public bool HasMoved { get; private set; }

    private static readonly (int Row, int Col)[] OrthogonalDirections =
    [
        (-1, 0), // North
        (1, 0),  // South
        (0, -1), // West
        (0, 1)   // East
    ];

    public Rook(PieceTeam team, BoardPosition position, int pieceNumber = 1)
        : base(team, position, pieceNumber)
    {
    }

    // For serialization
    public Rook() { }

    public override IReadOnlyList<BoardPosition> GetValidMoves(IChessBoard board)
    {
        var moves = new List<BoardPosition>();

        foreach (var (rowDelta, colDelta) in OrthogonalDirections)
        {
            AddSlidingMoves(board, moves, rowDelta, colDelta);
        }

        return moves;
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
