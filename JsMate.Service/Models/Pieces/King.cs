using System;
using System.Collections.Generic;

namespace JsMate.Service.Models
{
    public class King : ChessPiece
    {
        public King()
        {
        }


        public string PieceType => typeof(King).Name;

        public new List<BoardPosition> GetValidMoves()
        {
            var candidatePositions = new List<BoardPosition>();

            // N
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol - 1, BoardPosition.PositionRow));

            // NE
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol - 1, BoardPosition.PositionRow + 1));

            // E
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol, BoardPosition.PositionRow + 1));

            // SE
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol + 1, BoardPosition.PositionRow + 1));

            // S
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol + 1, BoardPosition.PositionRow));

            // SW
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol + 1, BoardPosition.PositionRow - 1));

            // W
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol, BoardPosition.PositionRow - 1));

            // NW
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol - 1, BoardPosition.PositionRow - 1));

            return candidatePositions;
        }
    }
}