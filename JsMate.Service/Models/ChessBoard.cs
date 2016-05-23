using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace JsMate.Service.Models
{
    public interface IChessBoard
    {
        List<IChessPiece> Pieces { get; set; }
    }

    public class ChessBoard : IChessBoard
    {
        // TODO: Consider removing this flag
        private bool _instantiated = false;

        public string Id { get; set; }
        public List<IChessPiece> Pieces { get; set; }

        public ChessBoard(string id, bool canCreateNew = true)
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
                        Pieces = foundBoard.Pieces;
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

            if (!canCreateNew)
            {
                throw new ApplicationException("Board not found.  Moves can only be applied to existing boards.");
            }

            try
            {
                // If not already create set up initial chessboard state
                if (Pieces == null) { Pieces = new List<IChessPiece>();}

                Pieces.AddRange(SetInitialBoardState());
                Id = id;
                using (var db = new LiteDatabase(@"ChessData.db"))
                {
                    var boards = db.GetCollection<ChessBoard>("chessBoards");
                    _instantiated = true;
                    boards.Insert(this);
                    boards.EnsureIndex(x => x.Id);
                }
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
            return Equals(Pieces, other.Pieces);
        }

        public override int GetHashCode()
        {
            return (Pieces != null ? Pieces.GetHashCode() : 0);
        }
        #endregion

        private List<ChessPiece> SetInitialBoardState()
        {
            var initialPieces = new List<ChessPiece>();
            // Add Pawns
            for (var i = 0; i < 8; i++)
            {
                initialPieces.Add(new Pawn(PieceTeam.White) { BoardPosition = new BoardPosition(6, i), PieceNumber = i + 1});
                initialPieces.Add(new Pawn(PieceTeam.Black) { BoardPosition = new BoardPosition(1, i), PieceNumber = i + 1 });
            }

            // Add White Rooks
            initialPieces.Add(new Rook() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 0), PieceNumber = 1 });
            initialPieces.Add(new Rook() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 7), PieceNumber = 2 });

            // Add Black Rooks
            initialPieces.Add(new Rook() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 0), PieceNumber = 1 });
            initialPieces.Add(new Rook() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 7), PieceNumber = 2 });

            // Add White Knights
            initialPieces.Add(new Knight() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 1), PieceNumber = 1 });
            initialPieces.Add(new Knight() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 6), PieceNumber = 2 });

            // Add Black Knights
            initialPieces.Add(new Knight() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 1), PieceNumber = 1 });
            initialPieces.Add(new Knight() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 6), PieceNumber = 2 });


            // Add White Bishops
            initialPieces.Add(new Bishop() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 2), PieceNumber = 1 });
            initialPieces.Add(new Bishop() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 5), PieceNumber = 2 });

            // Add Black Bishops
            initialPieces.Add(new Bishop() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 2), PieceNumber = 1 });
            initialPieces.Add(new Bishop() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 5), PieceNumber = 2 });

            // Add White Queen
            initialPieces.Add(new Queen() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 3), PieceNumber = 1 });

            // Add Black Queen
            initialPieces.Add(new Queen() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 3), PieceNumber = 1 });

            // Add White King
            initialPieces.Add(new King() { PieceTeam = PieceTeam.White, BoardPosition = new BoardPosition(7, 4), PieceNumber = 1 });

            // Add Black King
            initialPieces.Add(new King() { PieceTeam = PieceTeam.Black, BoardPosition = new BoardPosition(0, 4), PieceNumber = 1 });

            return initialPieces;
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
            return Pieces.All(piece => chessBoard.Pieces.Contains(piece)) &&
                   chessBoard.Pieces.All(piece2 => Pieces.Contains(piece2));
        }

        public bool Update(ChessBoard cb)
        {
            using (var db = new LiteDatabase(@"ChessData.db"))
            {
                try
                {
                    var boards = db.GetCollection<ChessBoard>("chessBoards");
                    return boards.Update(cb);
                   
                }
                catch (Exception)
                {
                    Console.WriteLine($"Failed to update existing board [{cb.Id}]");
                    return false;
                }
            }
        }

        public void ValidateFriendlyFire()
        {
            var result = Pieces.GroupBy(x => new { x.BoardPosition.Col,  x.BoardPosition.Row })
                .Where(g => g.Count() > 1)
                .Select(f => f.Key)
                .ToList();

            if (result.Any())
            {
                throw new InvalidOperationException("Friendly Fire!  Move not allowed!");
            }
        }
    }
}
