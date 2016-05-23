using System.Collections.Generic;

namespace JsMate.Service.Models.Pieces
{
    public class Rook : ChessPiece
    {
        public Rook()
        {
        }

        public override string PieceType => typeof(Rook).Name;
        
        public override List<BoardPosition> GetValidMoves()
        {
            var candidatePositions = new CandidatePositions();

            // N
            for (int n = 1; n < 7; n++)
            {
                candidatePositions.Add(new BoardPosition(BoardPosition.Col - n, BoardPosition.Row));
            }

            // E
            for (int e = 1; e < 7; e++)
            {
                candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row + e));
            }

            // S
            for (int s = 1; s < 7; s++)
            {
                candidatePositions.Add(new BoardPosition(BoardPosition.Col + s, BoardPosition.Row));
            }
            

            // W
            for (int w = 1; w < 7; w++)
            {
                candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row - w));
            }
            return candidatePositions.BoardPositions;
        }
    }
}