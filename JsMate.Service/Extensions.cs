using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsMate.Service
{
    public static class Extensions
    {
        public static void ValidateRowCol(this int? candiateRowCol)
        {
            if ((candiateRowCol < 0) || (candiateRowCol > 7))
                throw new ArgumentOutOfRangeException(nameof(candiateRowCol), "Not a valid chessboard location");
        }
    }
}
