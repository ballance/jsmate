using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace JsMate.Service.Models
{
    public class ChessPiece //: IChessPiece
    {
        public List<BoardPosition> GetValidMoves()
        {
            throw new NotImplementedException();
        }
        private bool _active = true;

        public BoardPosition BoardPosition { get; set; }
       

        public bool Active => _active;

        //public PieceType PieceType
        //{
        //    get { return _pieceType; }
        //    set { _pieceType = value; }
        //}

        public PieceTeam PieceTeam
        {
            get { return _pieceTeam; }
            set { _pieceTeam = value; }
        }

        //private PieceType _pieceType;
        private PieceTeam _pieceTeam;

        public void TakePiece()
        {
            BoardPosition.PositionCol = null;
            BoardPosition.PositionRow = null;
            _active = false;
        }

        public bool CanMove(int? destinationRow, int? destinationCol)
        {
            try
            {
                destinationRow.ValidateRowCol();
                destinationCol.ValidateRowCol();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}