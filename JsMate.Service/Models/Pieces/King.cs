using System.Collections.Generic;

namespace JsMate.Service.Models.Pieces
{
    public class King : ChessPiece
    {
        public King()
        {
            PieceNumber = 1;
        }


        public override string PieceType => typeof(King).Name;

        public override List<BoardPosition> GetValidMoves(ChessBoard board)
        {
            var candidatePositions = new CandidatePositions();

            // N
            candidatePositions.Add(new BoardPosition(BoardPosition.Col - 1, BoardPosition.Row), board);

            // NE
            candidatePositions.Add(new BoardPosition(BoardPosition.Col - 1, BoardPosition.Row + 1), board);

            // E
            candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row + 1), board);

            // SE
            candidatePositions.Add(new BoardPosition(BoardPosition.Col + 1, BoardPosition.Row + 1), board);

            // S
            candidatePositions.Add(new BoardPosition(BoardPosition.Col + 1, BoardPosition.Row), board);

            // SW
            candidatePositions.Add(new BoardPosition(BoardPosition.Col + 1, BoardPosition.Row - 1), board);

            // W
            candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row - 1), board);

            // NW
            candidatePositions.Add(new BoardPosition(BoardPosition.Col - 1, BoardPosition.Row - 1), board);

            return candidatePositions.BoardPositions;
        }
    }
}