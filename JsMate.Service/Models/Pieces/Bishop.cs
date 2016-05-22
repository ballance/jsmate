using System.Collections.Generic;

namespace JsMate.Service.Models
{
    public class Bishop : ChessPiece
    {
        public Bishop()
        {
        }

        public new List<BoardPosition> GetValidMoves()
        {
            var candidatePositions = new List<BoardPosition>();

            // TODO: Add how far the bishop can move

            // NE
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol - 1, BoardPosition.PositionRow + 1));

            // SE
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol + 1, BoardPosition.PositionRow + 1));

            // SW
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol + 1, BoardPosition.PositionRow - 1));

            // NW
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol - 1, BoardPosition.PositionRow - 1));

            return candidatePositions;
        }
    }
}