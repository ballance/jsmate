using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace JsMate.Service.Models
{
    public interface IChessBoard
    {
        List<ChessPiece> Pieces { get; set; }
    }

    public class ChessBoard : IChessBoard
    {
        // TODO: Consider removing this flag
        private bool _instantiated = false;

        private List<ChessPiece> _pieces = new List<ChessPiece>();

        public string Id { get; private set; }
        public List<ChessPiece> Pieces
        {
            get { return _pieces; }
            set { _pieces = value; }
        }

        public ChessBoard(string id)
        {
            var boardGuid = Convert.ToString(id);
            using (var db = new LiteDatabase(@"ChessData.db"))
            {
                try
                {
                    var boards = db.GetCollection<ChessBoard>("chessBoards");
                    var foundBoardCollection = boards.Find(b => b.Id.Equals(boardGuid));
                    if (foundBoardCollection.Any())
                    {
                        var foundBoard = foundBoardCollection.Single();
                        _pieces = foundBoard.Pieces;
                        Id = foundBoard.Id;
                        _instantiated = true;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"Failed to retrieve existing board [{boardGuid}]");
                }
            }

            if (_instantiated) return;

            try
            {
                // If not already create set up initial chessboard state
                _pieces.AddRange(SetInitialBoardState());
                Id = id;
                using (var db = new LiteDatabase(@"ChessData.db"))
                {
                    var boards = db.GetCollection<ChessBoard>("chessBoards");
                    boards.Insert(this);
                    boards.EnsureIndex(x => x.Id);
                }
                _instantiated = true;
            }
            catch (Exception ex)
            {
                _instantiated = false;
            }
        }
        public ChessBoard()
        {
            // Added to support LiteDB.  Should only be referenced that way.
        }

        #region Comparison overrides
        protected bool Equals(ChessBoard other)
        {
            return Equals(_pieces, other.Pieces);
        }

        public override int GetHashCode()
        {
            return (_pieces != null ? _pieces.GetHashCode() : 0);
        }
        #endregion

        private List<ChessPiece> SetInitialBoardState()
        {
            var initialPieces = new List<ChessPiece>();
            // Add Pawns
            for (var i = 0; i < 8; i++)
            {
                initialPieces.Add(new Pawn(PieceTeam.White) { BoardPosition = new BoardPosition(6, i) });
                initialPieces.Add(new Pawn(PieceTeam.Black) { BoardPosition = new BoardPosition(1, i) });
            }

            // Add White Rooks
            initialPieces.Add(new Rook() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 0) });
            initialPieces.Add(new Rook() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 7) });

            // Add Black Rooks
            initialPieces.Add(new Rook() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 0) });
            initialPieces.Add(new Rook() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 7) });

            // Add White Knights
            initialPieces.Add(new Knight() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 1) });
            initialPieces.Add(new Knight() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 6) });

            // Add Black Knights
            initialPieces.Add(new Knight() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 1) });
            initialPieces.Add(new Knight() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 6) });


            // Add White Bishops
            initialPieces.Add(new Bishop() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 2) });
            initialPieces.Add(new Bishop() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 5) });

            // Add Black Bishops
            initialPieces.Add(new Bishop() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 2) });
            initialPieces.Add(new Bishop() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 5) });

            // Add White Queen
            initialPieces.Add(new Queen() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 3) });

            // Add Black Queen
            initialPieces.Add(new Queen() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 3) });

            // Add White King
            initialPieces.Add(new King() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 4) });

            // Add Black King
            initialPieces.Add(new King() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 4) });

            return initialPieces;
        }

        public ChessBoard(List<ChessPiece> pieces, string id)
        {
            Id = id;
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


    }
}
