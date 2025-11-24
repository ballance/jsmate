using JsMate.Domain.Enums;
using JsMate.Domain.ValueObjects;

namespace JsMate.Domain.Entities.Pieces;

public sealed class Queen : ChessPiece
{
    public override PieceType Type => PieceType.Queen;

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

    public Queen(PieceTeam team, BoardPosition position, int pieceNumber = 1)
        : base(team, position, pieceNumber)
    {
    }

    // For serialization
    public Queen() { }

    public override IReadOnlyList<BoardPosition> GetValidMoves(IChessBoard board)
    {
        var moves = new List<BoardPosition>();

        foreach (var (rowDelta, colDelta) in AllDirections)
        {
            AddSlidingMoves(board, moves, rowDelta, colDelta);
        }

        return moves;
    }
}
