using System;

namespace JsMate.Service.Models
{
    public class BoardPosition
    {
        public BoardPosition()
        {
            // Default ctor added to make litedb happy
            AttackPosition = false;
        }
        public BoardPosition(int? row, int? col)
        {
            Row = row;
            Col = col;
        }

        public bool AttackPosition { get; set; }

        public int? Row { get; set; }

        public int? Col { get; set; }

        public void ValidateBoardBounds()
        {
            if ((Col >= 0 && Col <= 7) && (Row >= 0 && Row <= 7) == false)
            {
                throw new InvalidOperationException("Piece fell off the board.");
            }
        }
    }
}