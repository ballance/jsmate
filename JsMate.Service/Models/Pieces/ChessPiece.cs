using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace JsMate.Service.Models
{
    public interface IChessPiece
    {
        string PieceType { get; }
        BoardPosition BoardPosition { get; set; }
        bool Active { get; }
        PieceTeam PieceTeam { get; set; }
        List<BoardPosition> GetValidMoves();
        void TakePiece();
        bool CanMove(int? destinationRow, int? destinationCol);
    }

    public class ChessPiece : IChessPiece
    {
        public ChessPiece()
        {
            
        }

        public int PieceNumber { get; set; }

        public virtual string PieceType { get; }

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
            BoardPosition.Col = null;
            BoardPosition.Row = null;
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