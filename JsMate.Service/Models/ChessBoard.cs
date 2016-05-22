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
        List<ChessPiece> Pieces { get; set; }
    }

    public class ChessBoard : IChessBoard
    {
        private bool _created = false;
        protected bool Equals(ChessBoard other)
        {
            return Equals(_pieces, other.Pieces);
        }

        public override int GetHashCode()
        {
            return (_pieces != null ? _pieces.GetHashCode() : 0);
        }

        private List<ChessPiece> _pieces = new List<ChessPiece>();

        public ChessBoard()
        {
            if (_created) return;

            try
            {
                _pieces.AddRange(SetInitialBoardState());
                _created = true;
            }
            catch (Exception)
            {
                _created = false;
            }
            // If not already create set up initial chessboard state
           
        }

        private List<ChessPiece> SetInitialBoardState()
        {
            var initialPieces = new List<ChessPiece>();
            // Add Pawns
            for (var i = 0; i < 8; i++)
            {
                initialPieces.Add(new Pawn(PieceTeam.White) {BoardPosition = new BoardPosition(6, i)});
                initialPieces.Add(new Pawn(PieceTeam.Black) {BoardPosition = new BoardPosition(1, i)});
            }

            // Add White Rooks
            initialPieces.Add(new Rook() {PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 0)});
            initialPieces.Add(new Rook() {PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 7)});

            // Add Black Rooks
            initialPieces.Add(new Rook() {PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 0)});
            initialPieces.Add(new Rook() {PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 7)});

            // Add White Knights
            initialPieces.Add(new Knight() {PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 1)});
            initialPieces.Add(new Knight() {PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 6)});

            // Add Black Knights
            initialPieces.Add(new Knight() {PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 1)});
            initialPieces.Add(new Knight() {PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 6)});


            // Add White Bishops
            initialPieces.Add(new Bishop() {PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 2)});
            initialPieces.Add(new Bishop() {PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 5)});

            // Add Black Bishops
            initialPieces.Add(new Bishop() {PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 2)});
            initialPieces.Add(new Bishop() {PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 5)});

            // Add White Queen
            initialPieces.Add(new Queen() {PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 3)});

            // Add Black Queen
            initialPieces.Add(new Queen() {PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 3)});

            // Add White King
            initialPieces.Add(new King() {PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 4)});

            // Add Black King
            initialPieces.Add(new King() {PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 4)});

            return initialPieces;
        }

        public ChessBoard(List<ChessPiece> pieces)
        {
            // TODO: Add validation
            _pieces.AddRange(pieces);
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

            // TODO: Find a cleaner way to make this comparison.  Goal is to ensure the same pieces are in the same places on both boards.
            return _pieces.All(piece => chessBoard.Pieces.Contains(piece))  &&
                   chessBoard.Pieces.All(piece2 => _pieces.Contains(piece2));

        }

        public List<ChessPiece> Pieces
        {
            get { return _pieces; }
            set { _pieces = value; }
        }
    }
}
