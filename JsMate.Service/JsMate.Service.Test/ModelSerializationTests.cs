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

            Assert.AreEqual("{\"Pieces\":[{\"BoardPosition\":{\"PositionRow\":6,\"PositionCol\":0},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"PositionRow\":1,\"PositionCol\":0},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"PositionRow\":6,\"PositionCol\":1},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"PositionRow\":1,\"PositionCol\":1},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"PositionRow\":6,\"PositionCol\":2},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"PositionRow\":1,\"PositionCol\":2},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"PositionRow\":6,\"PositionCol\":3},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"PositionRow\":1,\"PositionCol\":3},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"PositionRow\":6,\"PositionCol\":4},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"PositionRow\":1,\"PositionCol\":4},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"PositionRow\":6,\"PositionCol\":5},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"PositionRow\":1,\"PositionCol\":5},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"PositionRow\":6,\"PositionCol\":6},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"PositionRow\":1,\"PositionCol\":6},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"PositionRow\":6,\"PositionCol\":7},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"PositionRow\":1,\"PositionCol\":7},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"PositionRow\":7,\"PositionCol\":0},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"PositionRow\":7,\"PositionCol\":7},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"PositionRow\":0,\"PositionCol\":0},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"PositionRow\":0,\"PositionCol\":7},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"PositionRow\":7,\"PositionCol\":1},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"PositionRow\":7,\"PositionCol\":6},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"PositionRow\":0,\"PositionCol\":1},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"PositionRow\":0,\"PositionCol\":6},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"PositionRow\":7,\"PositionCol\":2},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"PositionRow\":7,\"PositionCol\":5},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"PositionRow\":0,\"PositionCol\":2},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"PositionRow\":0,\"PositionCol\":5},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"PositionRow\":7,\"PositionCol\":3},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"PositionRow\":0,\"PositionCol\":3},\"Active\":true,\"PieceTeam\":1},{\"BoardPosition\":{\"PositionRow\":7,\"PositionCol\":4},\"Active\":true,\"PieceTeam\":0},{\"BoardPosition\":{\"PositionRow\":0,\"PositionCol\":4},\"Active\":true,\"PieceTeam\":1}]}", 
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
