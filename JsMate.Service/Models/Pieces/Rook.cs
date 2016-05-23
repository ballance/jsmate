using System;
using System.Collections.Generic;

namespace JsMate.Service.Models
{
    public class Rook : ChessPiece
    {
        public Rook()
        {
        }

        public override string PieceType => typeof(Rook).Name;
        
        public new List<BoardPosition> GetValidMoves()
        {
            var candidatePositions = new List<BoardPosition>();

            // N
            candidatePositions.Add(new BoardPosition(BoardPosition.Col - 1, BoardPosition.Row));
        
            // E
            candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row + 1));
        
            // S
            candidatePositions.Add(new BoardPosition(BoardPosition.Col + 1, BoardPosition.Row));

            // W
            candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row - 1));

            return candidatePositions;
        }
    }
}