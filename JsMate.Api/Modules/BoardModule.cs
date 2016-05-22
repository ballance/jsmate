using System;
using JsMate.Service.Models;
using Nancy;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JsMate.Api
{
    public class BoardModule : NancyModule
    {
        public BoardModule()
        {
            Get["/board/"] = parameters =>
            {
                IChessBoard cb = new ChessBoard();

                var ser = JsonConvert.SerializeObject(cb);
                return ser;
            };

            Get["/board/{id}"] = parameters => "received [" + parameters.Id.Value + "]";
        }
    }
}
