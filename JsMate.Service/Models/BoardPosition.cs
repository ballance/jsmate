using System;

namespace JsMate.Service.Models
{
    public class BoardPosition
    {
        public BoardPosition()
        {
            // Default ctor added to make litedb happy
        }
        public BoardPosition(int? row, int? col)
        {
            //row.ValidateRowCol();
            //col.ValidateRowCol();

            Row = row;
            Col = col;
        }

        public bool AttackPosition = false;

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