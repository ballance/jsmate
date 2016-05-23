using System;
using System.Collections.Generic;

namespace JsMate.Service.Models
{
    public class Rook : ChessPiece
    {
        public Rook()
        {
        }

        public string PieceType => typeof(Rook).Name;
        
        public new List<BoardPosition> GetValidMoves()
        {
            var candidatePositions = new List<BoardPosition>();

            // N
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol - 1, BoardPosition.PositionRow));
        
            // E
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol, BoardPosition.PositionRow + 1));
        
            // S
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol + 1, BoardPosition.PositionRow));

            // W
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol, BoardPosition.PositionRow - 1));

            return candidatePositions;
        }
    }
}