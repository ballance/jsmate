using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace JsMate.Service.Models
{
    public interface IChessBoard
    {
        char[,] Board { get; }
    }

    public class ChessBoard : IChessBoard
    {
        protected bool Equals(ChessBoard other)
        {
            return Equals(_board, other._board);
        }

        public override int GetHashCode()
        {
            return (_board != null ? _board.GetHashCode() : 0);
        }

        private char[,] _board = new char[8,8];

        public ChessBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    _board[i,j] = 'o';
                }
            }
            _board[4, 4] = 'x';
        }

        public override bool Equals(object obj)
        {  
            if (obj == null)
            {
                return false;
            }

            var chessBoard = obj as ChessBoard;
            if (chessBoard == null)
            {
                return false;
            }

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if ((_board[i, j].Equals(chessBoard.Board[i, j])) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public char[,] Board => _board;
    }
}
