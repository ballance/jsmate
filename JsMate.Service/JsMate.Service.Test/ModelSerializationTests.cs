using System;
using JsMate.Service.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JsMate.Service.Test
{
    [TestFixture]
    public class ModelSerializationTests
    {
        [Test]
        public void ShouldSerializeChessBoard()
        {
            var cb = new ChessBoard();
            var ser = JsonConvert.SerializeObject(cb);

            Assert.AreEqual("{\"Pieces\":[{\"BoardPosition\":{\"Row\":6,\"Col\":0},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"Row\":1,\"Col\":0},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"Row\":6,\"Col\":1},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"Row\":1,\"Col\":1},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"Row\":6,\"Col\":2},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"Row\":1,\"Col\":2},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"Row\":6,\"Col\":3},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"Row\":1,\"Col\":3},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"Row\":6,\"Col\":4},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"Row\":1,\"Col\":4},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"Row\":6,\"Col\":5},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"Row\":1,\"Col\":5},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"Row\":6,\"Col\":6},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"Row\":1,\"Col\":6},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"Row\":6,\"Col\":7},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"Row\":1,\"Col\":7},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"Row\":7,\"Col\":0},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"Row\":7,\"Col\":7},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"Row\":0,\"Col\":0},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"Row\":0,\"Col\":7},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"Row\":7,\"Col\":1},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"Row\":7,\"Col\":6},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"Row\":0,\"Col\":1},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"Row\":0,\"Col\":6},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"Row\":7,\"Col\":2},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"Row\":7,\"Col\":5},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"Row\":0,\"Col\":2},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"Row\":0,\"Col\":5},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"Row\":7,\"Col\":3},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"Row\":0,\"Col\":3},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"Row\":7,\"Col\":4},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"Row\":0,\"Col\":4},\"Active\":true,\"PieceTeam\":1}]}", 
                ser);
        }

        [Test]
        public void ShouldDeserializeChessBoard()
        {
            var chessBoard = new ChessBoard();
            var serializedChessBoard = JsonConvert.SerializeObject(chessBoard);
            var deserializedChessBoard = JsonConvert.DeserializeObject<ChessBoard>(serializedChessBoard);

            Assert.AreEqual(chessBoard, deserializedChessBoard);
        }
    }
}
