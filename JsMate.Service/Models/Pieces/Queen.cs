using System;
using System.Collections.Generic;

namespace JsMate.Service.Models
{
    public class Queen : ChessPiece
    {
        public Queen()
        {
            PieceNumber = 1;
        }

        public override string PieceType => typeof(Queen).Name;

        public new List<BoardPosition> GetValidMoves()
        {
            var candidatePositions = new List<BoardPosition>();

            // TODO: Add how far the queen can move

            // N
            candidatePositions.Add(new BoardPosition(BoardPosition.Col - 1, BoardPosition.Row));

            // NE
            candidatePositions.Add(new BoardPosition(BoardPosition.Col - 1, BoardPosition.Row + 1));

            // E
            candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row + 1));

            // SE
            candidatePositions.Add(new BoardPosition(BoardPosition.Col + 1, BoardPosition.Row + 1));

            // S
            candidatePositions.Add(new BoardPosition(BoardPosition.Col + 1, BoardPosition.Row));

            // SW
            candidatePositions.Add(new BoardPosition(BoardPosition.Col + 1, BoardPosition.Row - 1));

            // W
            candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row - 1));

            // NW
            candidatePositions.Add(new BoardPosition(BoardPosition.Col - 1, BoardPosition.Row - 1));

            return candidatePositions;
        }
    }
}