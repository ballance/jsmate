using System.Collections.Generic;

namespace JsMate.Service.Models.Pieces
{
    public class Queen : ChessPiece
    {
        public Queen()
        {
            PieceNumber = 1;
        }

        public override string PieceType => typeof(Queen).Name;

        public override List<BoardPosition> GetValidMoves()
        {
            var candidatePositions = new CandidatePositions();

            // TODO: Add how far the queen can move

            // N
            for (int n = 1; n < 7; n++)
            {
                candidatePositions.Add(new BoardPosition(BoardPosition.Col - n, BoardPosition.Row));
            }

            // NE
            for (int ne = 1; ne < 7; ne++)
            {
                candidatePositions.Add(new BoardPosition(BoardPosition.Col - ne, BoardPosition.Row + ne));
            }

            // E
            for (int e = 1; e < 7; e++)
            {
                candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row + e));
            }

            // SE
            for (int se = 1; se < 7; se++)
            {
                candidatePositions.Add(new BoardPosition(BoardPosition.Col + se, BoardPosition.Row + se));
            }

            // S
            for (int s = 1; s < 7; s++)
            {
                candidatePositions.Add(new BoardPosition(BoardPosition.Col + s, BoardPosition.Row));
            }

            // SW
            for (int sw = 1; sw < 7; sw++)
            {
                candidatePositions.Add(new BoardPosition(BoardPosition.Col + sw, BoardPosition.Row - sw));
            }

            // W
            for (int w = 1; w < 7; w++)
            {
                candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row - w));
            }

            // NW
            for (int nw = 1; nw < 7; nw++)
            {
                candidatePositions.Add(new BoardPosition(BoardPosition.Col - nw, BoardPosition.Row - nw));
            }

            return candidatePositions.BoardPositions;
        }
    }
}