using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace JsMate.Service.Models.Pieces
{
    public class CandidatePositions
    {
        public CandidatePositions()
        {
            BoardPositions = new List<BoardPosition>();
        }
        
        public List<BoardPosition> BoardPositions { get; set; }

        public bool Add(BoardPosition newPosition, ChessBoard board)
        {
            try
            {
                if (newPosition.AttackPosition == false &&
                    board.Pieces.Any(
                        x => x.BoardPosition.Col.Equals(newPosition.Row) && x.BoardPosition.Row.Equals(newPosition.Col)))
                    return false;

                BoardPositions.Add(newPosition);
                return true;
            }
            catch (Exception)
            {
                return false;
                // TODO: Ignore for now.  Perhaps add trace logging here at some point.
            }
        }
    }
}