using System;
using JsMate.Service.Models.Pieces;

namespace JsMate.Service.Models
{
    public class ChessSquare
    {
        public bool HasPiece => true;
        private ChessPiece _piece;

        public ChessPiece Piece
        {
            get { return _piece; }
        }

        public bool SetPiece(ChessPiece piece)
        {
            try
            {
                if (!HasPiece && Piece == null)
                {
                    _piece = piece;
                } 
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}