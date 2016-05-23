using System;
using System.Collections.Generic;

namespace JsMate.Service.Models.Pieces
{
    public class CandidatePositions
    {

        private List<BoardPosition> _candidatePositions = new List<BoardPosition>();

        public CandidatePositions()
        {
            
        }
        
        public List<BoardPosition> BoardPositions => _candidatePositions;

        public void Add(BoardPosition newPosition)
        {
            try
            {
                _candidatePositions.Add(newPosition);
            }
            catch (Exception)
            {
                // TODO: Ignore for now.  Perhaps add trace logging here at some point.
            }
        }
    }
}