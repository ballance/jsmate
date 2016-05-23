using System.Collections.Generic;

namespace JsMate.Service.Models.Pieces
{
    public class Knight : ChessPiece
    {
        public Knight()
        {
        }


        public override string PieceType => typeof(Knight).Name;

        public override List<BoardPosition> GetValidMoves(ChessBoard board)
        {
            var candidatePositions = new CandidatePositions();

            // NW
            candidatePositions.Add(new BoardPosition(BoardPosition.Col-1, BoardPosition.Row-2), board);

            // NE            
            candidatePositions.Add(new BoardPosition(BoardPosition.Col+1, BoardPosition.Row-2), board);

            // EN
            candidatePositions.Add(new BoardPosition(BoardPosition.Col+2, BoardPosition.Row+1), board);

            // ES
            candidatePositions.Add(new BoardPosition(BoardPosition.Col+2, BoardPosition.Row-1), board);

            // SE
            candidatePositions.Add(new BoardPosition(BoardPosition.Col+1, BoardPosition.Row+2), board);

            // SW
            candidatePositions.Add(new BoardPosition(BoardPosition.Col-1, BoardPosition.Row+2), board);

            // WN
            candidatePositions.Add(new BoardPosition(BoardPosition.Col-2, BoardPosition.Row+1), board);

            // WS
            candidatePositions.Add(new BoardPosition(BoardPosition.Col-2, BoardPosition.Row-1), board);

            return candidatePositions.BoardPositions;
        }
    }
}