using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JsMate.Service.Models;
using LiteDB;
using Nancy;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JsMate.Api
{
    public class MoveModule : NancyModule
    {
        public MoveModule()
        {
            // Default Board
            Get["/move/"] = parameters => HttpStatusCode.BadRequest;

            // Attempt to load existing board, create new if none found
            Get["/move/{id}/{team}/{piece}"] = parameters =>
            {
                var boardId = parameters.Id.Value;
                var team = parameters.Team.Value;
                var piece = parameters.Piece.Value;
                try
                {
                    ChessBoard foundBoard = FindBoard(boardId);

                    //var pieceToMove =
                    //    foundBoard.Pieces.Single(
                    //        x => x.PieceType.Equals(piece) && x.PieceTeam.Equals(team));

                    var pieceToMove =
                        foundBoard.Pieces.Single(
                            x => x.PieceType.Equals(piece) && x.PieceTeam.Equals(PieceTeam.Black));
                    
                    pieceToMove.BoardPosition.Row++;


                    foundBoard.ValidateFriendlyFire();
                    pieceToMove.BoardPosition.ValidateBoardBounds();
                    
                    foundBoard.Update(foundBoard);

                    Console.WriteLine($"Completed move for [{boardId}]");
                    return JsonConvert.SerializeObject(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Move failed for [{boardId} / {team} / {piece}] " + ex.Message );
                    return JsonConvert.SerializeObject(false);
                }
            };
        }
        private ChessBoard FindBoard(string boardId)
        {
            try
            {
                Console.WriteLine($"Attempt to load existing board [{boardId}], create new if none found");
                var cb = new ChessBoard(boardId, false);
                return (ChessBoard)cb;
            }
            catch (Exception)
            {
                Console.WriteLine($"Failed to create board for [{boardId}]");
                throw;
            }
        }
    }
}
