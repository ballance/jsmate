using System.Collections.Generic;

namespace JsMate.Service.Models.Pieces
{
    public class Bishop : ChessPiece
    {
        public Bishop()
        {
        }

        public override string PieceType => typeof(Bishop).Name;

        public override List<BoardPosition> GetValidMoves(ChessBoard board)
        {
            var candidatePositions = new CandidatePositions();

            // TODO: Add how far the bishop can move

            // NE
            for (var ne = 1; ne < 7; ne++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col - ne, BoardPosition.Row + ne), board) == false)
                    break;
            }

            // SE
            for (var se = 1; se < 7; se++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col + se, BoardPosition.Row + se), board) == false)
                    break;
            }

            // SW
            for (var sw = 1; sw < 7; sw++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col + sw, BoardPosition.Row - sw), board) == false)
                    break;
            }

            // NW
            for (var nw = 1; nw < 7; nw++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col - nw, BoardPosition.Row - nw), board) ==
                    false)
                    break;
            }
            return candidatePositions.BoardPositions;
        }
    }
}