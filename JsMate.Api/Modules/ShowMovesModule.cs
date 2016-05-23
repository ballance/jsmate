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
    public class ShowMovesModule : NancyModule
    {
        public ShowMovesModule()
        {
            // Default Board
            Get["/showmoves/"] = parameters => HttpStatusCode.BadRequest;

            // Attempt to load existing board, create new if none found
            Get["/showmoves/{id}/{selectedSquare}"] = parameters =>
            {
                var boardId = parameters.Id.Value;
                string squareCoordinates = parameters.selectedSquare.Value;
                var squareCoordinatesSplit = squareCoordinates.Split('-');
                // TODO: This assumes a very specific format and is very brittle
                var col = Convert.ToInt32(squareCoordinatesSplit[1]);
                var row = Convert.ToInt32(squareCoordinatesSplit[0]);

                try
                {
                    ChessBoard foundBoard = FindBoard(boardId);

                    // TODO: Clean up these messy conversions
                    var pieceToEnumerateMovesFor =
                        foundBoard.Pieces.Single(
                            x => x.BoardPosition.Col.Equals(col) 
                                && x.BoardPosition.Row.Equals(row));

                    foundBoard.ValidateCollision();
                    pieceToEnumerateMovesFor.BoardPosition.ValidateBoardBounds();
                    
                    Console.WriteLine($"Enumerating moves for [{boardId}]");

                    var candidateMoves = pieceToEnumerateMovesFor.GetValidMoves(foundBoard);

                    var collisionsRemoved = foundBoard.RemoveCollisions(candidateMoves);

                    return JsonConvert.SerializeObject(collisionsRemoved);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ShowMoves failed for [{boardId} / {squareCoordinates}] " + ex.Message );
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
