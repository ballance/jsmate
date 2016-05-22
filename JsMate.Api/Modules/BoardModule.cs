using System;
using JsMate.Service.Models;
using LiteDB;
using Nancy;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JsMate.Api
{
    public class BoardModule : NancyModule
    {
        public BoardModule()
        {
            // Default Board
            Get["/board/"] = parameters =>
            {
                IChessBoard cb = new ChessBoard(Guid.NewGuid().ToString());

                var ser = JsonConvert.SerializeObject(cb);
                return ser;
            };

            // Attempt to load existing board, create new if none found
            Get["/board/{id}"] = parameters =>
            {
                var boardId = parameters.Id.Value;
              
                IChessBoard cb = new ChessBoard(boardId);

                var ser = JsonConvert.SerializeObject(cb);
                return ser;
            };

            //Get["/board/{id}"] = parameters => "received [" + parameters.Id.Value + "]";
        }

     
    }
}
