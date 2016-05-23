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
            Get["/move/{id}/{team}/{piece}/{pieceNumber}"] = parameters =>
            {
                var boardId = parameters.Id.Value;
                var team = parameters.Team.Value;
                var piece = parameters.Piece.Value;
                var pieceNumber = Convert.ToInt32(parameters.PieceNumber.Value);
                
                try
                {
                    ChessBoard foundBoard = FindBoard(boardId);

                    // TODO: Clean up these messy conversions
                    var piecesToMove =
                        foundBoard.Pieces.Where(
                            x => x.PieceType.Equals(piece) 
                                && x.PieceTeam.Equals(team == "0" ? PieceTeam.Black : PieceTeam.White)
                                //&& x.PieceNumber.Equals(pieceNumber)
                                );

                    foreach (var pieceToMove in piecesToMove)
                    {
                        // TODO: Ick, but it shows both white & black pieces moving forward
                        if (pieceToMove.PieceTeam.Equals(PieceTeam.Black))
                            pieceToMove.BoardPosition.Row++;
                        else
                        {
                            pieceToMove.BoardPosition.Row--;
                        }

                        foundBoard.ValidateCollision();
                        pieceToMove.BoardPosition.ValidateBoardBounds();

                        foundBoard.Update(foundBoard);
                    }
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
