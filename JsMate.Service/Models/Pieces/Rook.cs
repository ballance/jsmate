using System.Collections.Generic;

namespace JsMate.Service.Models.Pieces
{
    public class Rook : ChessPiece
    {
        public Rook()
        {
        }

        public override string PieceType => typeof(Rook).Name;
        
        // TODO: I don't like the encapsulation here that has me passing around the chess board to check each new potential move.  This could be structured better.  Validations should probably be happening on the board itself.
        public override List<BoardPosition> GetValidMoves(ChessBoard board)
        {
            var candidatePositions = new CandidatePositions();

            // N
            for (var n = 1; n < 7; n++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col - n, BoardPosition.Row), board) == false)
                    break;
            }

            // E
            for (var e = 1; e < 7; e++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row + e), board) == false)
                    break;
            }

            // S
            for (var s = 1; s < 7; s++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col + s, BoardPosition.Row), board) == false)
                    break;
            }
            

            // W
            for (var w = 1; w < 7; w++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row - w), board) == false)
                    break;
            }
            return candidatePositions.BoardPositions;
        }
    }
}