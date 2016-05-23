using System;
using System.Collections.Generic;

namespace JsMate.Service.Models
{
    public class Bishop : ChessPiece
    {
        public Bishop()
        {
        }

        public override string PieceType => typeof(Bishop).Name;

        public new List<BoardPosition> GetValidMoves()
        {
            var candidatePositions = new List<BoardPosition>();

            // TODO: Add how far the bishop can move

            // NE
            candidatePositions.Add(new BoardPosition(BoardPosition.Col - 1, BoardPosition.Row + 1));

            // SE
            candidatePositions.Add(new BoardPosition(BoardPosition.Col + 1, BoardPosition.Row + 1));

            // SW
            candidatePositions.Add(new BoardPosition(BoardPosition.Col + 1, BoardPosition.Row - 1));

            // NW
            candidatePositions.Add(new BoardPosition(BoardPosition.Col - 1, BoardPosition.Row - 1));

            return candidatePositions;
        }
    }
}