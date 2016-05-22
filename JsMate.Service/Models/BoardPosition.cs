namespace JsMate.Service.Models
{
    public class BoardPosition
    {
        public BoardPosition(int? row, int? col)
        {
            row.ValidateRowCol();
            col.ValidateRowCol();

            PositionRow = row;
            PositionCol = col;
        }

        public int? PositionRow { get; set; }

        public int? PositionCol { get; set; }
    }
}