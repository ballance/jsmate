using JsMate.Domain.Enums;
using JsMate.Domain.ValueObjects;

namespace JsMate.Domain.Entities.Pieces;

public sealed class Bishop : ChessPiece
{
    public override PieceType Type => PieceType.Bishop;

    private static readonly (int Row, int Col)[] DiagonalDirections =
    [
        (-1, -1), // NW
        (-1, 1),  // NE
        (1, -1),  // SW
        (1, 1)    // SE
    ];

    public Bishop(PieceTeam team, BoardPosition position, int pieceNumber = 1)
        : base(team, position, pieceNumber)
    {
    }

    // For serialization
    public Bishop() { }

    public override IReadOnlyList<BoardPosition> GetValidMoves(IChessBoard board)
    {
        var moves = new List<BoardPosition>();

        foreach (var (rowDelta, colDelta) in DiagonalDirections)
        {
            AddSlidingMoves(board, moves, rowDelta, colDelta);
        }

        return moves;
    }
}
