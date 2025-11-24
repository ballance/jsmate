using JsMate.Domain.Enums;
using JsMate.Domain.ValueObjects;

namespace JsMate.Domain.Entities.Pieces;

public sealed class Knight : ChessPiece
{
    public override PieceType Type => PieceType.Knight;

    private static readonly (int Row, int Col)[] KnightMoves =
    [
        (-2, -1), (-2, 1),  // Up 2, left/right 1
        (-1, -2), (-1, 2),  // Up 1, left/right 2
        (1, -2), (1, 2),    // Down 1, left/right 2
        (2, -1), (2, 1)     // Down 2, left/right 1
    ];

    public Knight(PieceTeam team, BoardPosition position, int pieceNumber = 1)
        : base(team, position, pieceNumber)
    {
    }

    // For serialization
    public Knight() { }

    public override IReadOnlyList<BoardPosition> GetValidMoves(IChessBoard board)
    {
        var moves = new List<BoardPosition>();

        foreach (var (rowDelta, colDelta) in KnightMoves)
        {
            var newPos = BoardPosition.TryCreate(Position.Row + rowDelta, Position.Col + colDelta);
            if (newPos.HasValue && !IsBlockedByOwnPiece(board, newPos.Value))
            {
                moves.Add(newPos.Value);
            }
        }

        return moves;
    }
}
