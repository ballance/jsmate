using System;
using System.Collections.Generic;

namespace JsMate.Service.Models
{
    public class Knight : ChessPiece
    {
        public Knight()
        {
        }


        public string PieceType => typeof(Knight).Name;

        public new List<BoardPosition> GetValidMoves()
        {
            var candidatePositions = new List<BoardPosition>();

            // NW
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol-1, BoardPosition.PositionRow-2));

            // NE            
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol-1, BoardPosition.PositionRow-2));

            // EN
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol+2, BoardPosition.PositionRow+1));

            // ES
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol+2, BoardPosition.PositionRow-1));

            // SE
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol+1, BoardPosition.PositionRow+2));

            // SW
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol-1, BoardPosition.PositionRow+2));

            // WN
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol-2, BoardPosition.PositionRow+1));

            // WS
            candidatePositions.Add(new BoardPosition(BoardPosition.PositionCol-2, BoardPosition.PositionRow-1));

            return candidatePositions;
        }
    }
}