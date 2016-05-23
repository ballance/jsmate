using System.Collections.Generic;

namespace JsMate.Service.Models.Pieces
{
    public class Pawn : ChessPiece 
    {
        public Pawn(PieceTeam team)
        {
            PieceTeam = team;
        }

        public Pawn()
        {
        }

        public override string PieceType => typeof(Pawn).Name;

        public override List<BoardPosition> GetValidMoves(ChessBoard board)
        {
            var candidatePositions = new CandidatePositions();
            var direction = 1;
            if (PieceTeam == PieceTeam.White)
            {
                direction = -1;
            }

            // TODO: This is loop is dangerous and inconvenient, but I do love Fig Newtons.
            for (var i = 1; i <= 2; i++)
            {
                if (candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row + direction*i), board) == false)
                    break;
            }

            // Take East (check for collision before allowing)
            candidatePositions.Add(new BoardPosition(BoardPosition.Col + direction, BoardPosition.Row + direction) { AttackPosition = true}, board);

            // Take west (check for collision before allowing)
            candidatePositions.Add(new BoardPosition(BoardPosition.Col + -1*direction, BoardPosition.Row + direction) { AttackPosition = true }, board);
            
            return candidatePositions.BoardPositions;
        }
    }
}