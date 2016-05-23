using System;
using System.Collections.Generic;

namespace JsMate.Service.Models
{
    public class Knight : ChessPiece
    {
        public Knight()
        {
        }


        public override string PieceType => typeof(Knight).Name;

        public new List<BoardPosition> GetValidMoves()
        {
            var candidatePositions = new List<BoardPosition>();

            // NW
            candidatePositions.Add(new BoardPosition(BoardPosition.Col-1, BoardPosition.Row-2));

            // NE            
            candidatePositions.Add(new BoardPosition(BoardPosition.Col-1, BoardPosition.Row-2));

            // EN
            candidatePositions.Add(new BoardPosition(BoardPosition.Col+2, BoardPosition.Row+1));

            // ES
            candidatePositions.Add(new BoardPosition(BoardPosition.Col+2, BoardPosition.Row-1));

            // SE
            candidatePositions.Add(new BoardPosition(BoardPosition.Col+1, BoardPosition.Row+2));

            // SW
            candidatePositions.Add(new BoardPosition(BoardPosition.Col-1, BoardPosition.Row+2));

            // WN
            candidatePositions.Add(new BoardPosition(BoardPosition.Col-2, BoardPosition.Row+1));

            // WS
            candidatePositions.Add(new BoardPosition(BoardPosition.Col-2, BoardPosition.Row-1));

            return candidatePositions;
        }
    }
}