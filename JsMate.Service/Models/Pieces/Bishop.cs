using System.Collections.Generic;

namespace JsMate.Service.Models.Pieces
{
    public class Bishop : ChessPiece
    {
        public Bishop()
        {
        }

        public override string PieceType => typeof(Bishop).Name;

        public override List<BoardPosition> GetValidMoves()
        {
            var candidatePositions = new List<BoardPosition>();

            // TODO: Add how far the bishop can move

            // NE
            for (int ne = 1; ne < 7; ne++)
            {
                candidatePositions.Add(new BoardPosition(BoardPosition.Col - ne, BoardPosition.Row + ne));
            }

            // SE
            for (int se = 1; se < 7; se++)
            {
                candidatePositions.Add(new BoardPosition(BoardPosition.Col + se, BoardPosition.Row + se));
            }

            // SW
            for (int sw = 1; sw < 7; sw++)
            {
                candidatePositions.Add(new BoardPosition(BoardPosition.Col + sw, BoardPosition.Row - sw));
            }

            // NW
            for (int nw = 1; nw < 7; nw++)
            {
                candidatePositions.Add(new BoardPosition(BoardPosition.Col - nw, BoardPosition.Row - nw));
            }
            return candidatePositions;
        }
    }
}