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

        public override List<BoardPosition> GetValidMoves(ChessBoard board)
        {
            var candidatePositions = new CandidatePositions();

            // TODO: Add how far the queen can move

            var moveDepth = 15;
            // N
            for (int n = 1; n < moveDepth; n++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col - n, BoardPosition.Row), board) == false)
                    break;
            }

            // NE
            for (int ne = 1; ne < moveDepth; ne++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col - ne, BoardPosition.Row + ne), board) == false)
                    break;
            }

            // E
            for (int e = 1; e < moveDepth; e++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row + e), board) == false)
                    break;
            }

            // SE
            for (int se = 1; se < moveDepth; se++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col + se, BoardPosition.Row + se), board) == false)
                    break;
            }

            // S
            for (int s = 1; s < moveDepth; s++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col + s, BoardPosition.Row), board) == false)
                    break;
            }

            // SW
            for (int sw = 1; sw < moveDepth; sw++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col + sw, BoardPosition.Row - sw), board) == false)
                    break;
            }

            // W
            for (int w = 1; w < moveDepth; w++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row - w), board) == false)
                    break;
            }

            // NW
            for (int nw = 1; nw < moveDepth; nw++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col - nw, BoardPosition.Row - nw), board) == false)
                    break;
            }

            return candidatePositions.BoardPositions;
        }
    }
}