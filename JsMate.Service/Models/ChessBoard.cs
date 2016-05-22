using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace JsMate.Service.Models
{
    public interface IChessBoard
    {
        List<ChessPiece> Pieces { get; }
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
            // Setting up initial chessboard state

            // Add Pawns
            //for (var i = 0; i < 8; i++)
            //{
            //    _pieces.Add(new Pawn(PieceTeam.White) { BoardPosition = new BoardPosition(6, i) });
            //    _pieces.Add(new Pawn(PieceTeam.Black) { BoardPosition = new BoardPosition(1, i) });
            //}

            //// Add White Rooks
            //_pieces.Add(new Rook() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 0) });
            //_pieces.Add(new Rook() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 7) });

            //// Add Black Rooks
            //_pieces.Add(new Rook() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 0)});
            //_pieces.Add(new Rook() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 7) });

            //// Add White Knights
            //_pieces.Add(new Knight() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 1)});
            //_pieces.Add(new Knight() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 6)});

            //// Add Black Knights
            //_pieces.Add(new Knight() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 1) });
            //_pieces.Add(new Knight() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 6) });


            //// Add White Bishops
            //_pieces.Add(new Bishop() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 2) });
            //_pieces.Add(new Bishop() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 5) });

            //// Add Black Bishops
            //_pieces.Add(new Bishop() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 2) });
            //_pieces.Add(new Bishop() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 5) });

            //// Add White Queen
            //_pieces.Add(new Queen() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 3) });

            //// Add Black Queen
            //_pieces.Add(new Queen() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 3) });

            // Add White King
            _pieces.Add(new King() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 4) });

            // Add Black King
            _pieces.Add(new King() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 4) });

            _created = true;
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
            return _pieces.All(piece => chessBoard.Pieces.Contains(piece)) &&
                   chessBoard.Pieces.All(piece2 => _pieces.Contains(piece2));

        }

        public List<ChessPiece> Pieces => _pieces;

    }
}
