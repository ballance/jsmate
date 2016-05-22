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

            Assert.AreEqual("{\"Board\":[[\"o\",\"o\",\"o\",\"o\",\"o\",\"o\",\"o\",\"o\"],[\"o\",\"o\",\"o\",\"o\",\"o\",\"o\",\"o\",\"o\"],[\"o\",\"o\",\"o\",\"o\",\"o\",\"o\",\"o\",\"o\"],[\"o\",\"o\",\"o\",\"o\",\"o\",\"o\",\"o\",\"o\"],[\"o\",\"o\",\"o\",\"o\",\"o\",\"o\",\"o\",\"o\"],[\"o\",\"o\",\"o\",\"o\",\"o\",\"o\",\"o\",\"o\"],[\"o\",\"o\",\"o\",\"o\",\"o\",\"o\",\"o\",\"o\"],[\"o\",\"o\",\"o\",\"o\",\"o\",\"o\",\"o\",\"o\"]]}", 
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
