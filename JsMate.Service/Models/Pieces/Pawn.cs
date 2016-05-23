using System;
using System.Collections.Generic;

namespace JsMate.Service.Models
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

        public new List<BoardPosition> GetValidMoves()
        {
            var candidatePositions = new List<BoardPosition>();
            var direction = 1;
            if (PieceTeam == PieceTeam.White)
            {
                direction = -1;
            }
            candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row + direction));
            candidatePositions.Add(new BoardPosition(BoardPosition.Col, BoardPosition.Row + 2 * direction));

            // Take East (check for collision before allowing)
            candidatePositions.Add(new BoardPosition(BoardPosition.Col + direction, BoardPosition.Row + direction));

            // Take west (check for collision before allowing)
            candidatePositions.Add(new BoardPosition(BoardPosition.Col + direction, BoardPosition.Row + direction));
            
            return candidatePositions;
        }
    }
}