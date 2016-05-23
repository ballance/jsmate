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
            var boardGuid = Guid.NewGuid().ToString();
            var cb = new ChessBoard(boardGuid);
            var ser = JsonConvert.SerializeObject(cb);

            String.Format("{0}", boardGuid);

            Assert.AreEqual("{\"Id\":\"" + boardGuid + "\",\"Pieces\":[{\"PieceType\":\"Pawn\",\"PieceNumber\":1,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":6,\"Col\":0},\"Active\":true,\"PieceTeam\":1},{\"PieceType\":\"Pawn\",\"PieceNumber\":1,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":1,\"Col\":0},\"Active\":true,\"PieceTeam\":0},{\"PieceType\":\"Pawn\",\"PieceNumber\":2,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":6,\"Col\":1},\"Active\":true,\"PieceTeam\":1},{\"PieceType\":\"Pawn\",\"PieceNumber\":2,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":1,\"Col\":1},\"Active\":true,\"PieceTeam\":0},{\"PieceType\":\"Pawn\",\"PieceNumber\":3,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":6,\"Col\":2},\"Active\":true,\"PieceTeam\":1},{\"PieceType\":\"Pawn\",\"PieceNumber\":3,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":1,\"Col\":2},\"Active\":true,\"PieceTeam\":0},{\"PieceType\":\"Pawn\",\"PieceNumber\":4,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":6,\"Col\":3},\"Active\":true,\"PieceTeam\":1},{\"PieceType\":\"Pawn\",\"PieceNumber\":4,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":1,\"Col\":3},\"Active\":true,\"PieceTeam\":0},{\"PieceType\":\"Pawn\",\"PieceNumber\":5,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":6,\"Col\":4},\"Active\":true,\"PieceTeam\":1},{\"PieceType\":\"Pawn\",\"PieceNumber\":5,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":1,\"Col\":4},\"Active\":true,\"PieceTeam\":0},{\"PieceType\":\"Pawn\",\"PieceNumber\":6,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":6,\"Col\":5},\"Active\":true,\"PieceTeam\":1},{\"PieceType\":\"Pawn\",\"PieceNumber\":6,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":1,\"Col\":5},\"Active\":true,\"PieceTeam\":0},{\"PieceType\":\"Pawn\",\"PieceNumber\":7,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":6,\"Col\":6},\"Active\":true,\"PieceTeam\":1},{\"PieceType\":\"Pawn\",\"PieceNumber\":7,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":1,\"Col\":6},\"Active\":true,\"PieceTeam\":0},{\"PieceType\":\"Pawn\",\"PieceNumber\":8,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":6,\"Col\":7},\"Active\":true,\"PieceTeam\":1},{\"PieceType\":\"Pawn\",\"PieceNumber\":8,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":1,\"Col\":7},\"Active\":true,\"PieceTeam\":0},{\"PieceType\":\"Rook\",\"PieceNumber\":1,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":7,\"Col\":0},\"Active\":true,\"PieceTeam\":1},{\"PieceType\":\"Rook\",\"PieceNumber\":2,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":7,\"Col\":7},\"Active\":true,\"PieceTeam\":1},{\"PieceType\":\"Rook\",\"PieceNumber\":1,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":0,\"Col\":0},\"Active\":true,\"PieceTeam\":0},{\"PieceType\":\"Rook\",\"PieceNumber\":2,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":0,\"Col\":7},\"Active\":true,\"PieceTeam\":0},{\"PieceType\":\"Knight\",\"PieceNumber\":1,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":7,\"Col\":1},\"Active\":true,\"PieceTeam\":1},{\"PieceType\":\"Knight\",\"PieceNumber\":2,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":7,\"Col\":6},\"Active\":true,\"PieceTeam\":1},{\"PieceType\":\"Knight\",\"PieceNumber\":1,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":0,\"Col\":1},\"Active\":true,\"PieceTeam\":0},{\"PieceType\":\"Knight\",\"PieceNumber\":2,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":0,\"Col\":6},\"Active\":true,\"PieceTeam\":0},{\"PieceType\":\"Bishop\",\"PieceNumber\":1,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":7,\"Col\":2},\"Active\":true,\"PieceTeam\":1},{\"PieceType\":\"Bishop\",\"PieceNumber\":2,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":7,\"Col\":5},\"Active\":true,\"PieceTeam\":1},{\"PieceType\":\"Bishop\",\"PieceNumber\":1,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":0,\"Col\":2},\"Active\":true,\"PieceTeam\":0},{\"PieceType\":\"Bishop\",\"PieceNumber\":2,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":0,\"Col\":5},\"Active\":true,\"PieceTeam\":0},{\"PieceType\":\"Queen\",\"PieceNumber\":1,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":7,\"Col\":3},\"Active\":true,\"PieceTeam\":1},{\"PieceType\":\"Queen\",\"PieceNumber\":1,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":0,\"Col\":3},\"Active\":true,\"PieceTeam\":0},{\"PieceType\":\"King\",\"PieceNumber\":1,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":7,\"Col\":4},\"Active\":true,\"PieceTeam\":1},{\"PieceType\":\"King\",\"PieceNumber\":1,\"BoardPosition\":{\"AttackPosition\":false,\"Row\":0,\"Col\":4},\"Active\":true,\"PieceTeam\":0}]}", 
                ser);
        }

        [Test]
        public void ShouldDeserializeChessBoard()
        {
            var boardGuid = Guid.NewGuid().ToString();
            var chessBoard = new ChessBoard(boardGuid);
            var serializedChessBoard = JsonConvert.SerializeObject(chessBoard);
            var deserializedChessBoard = JsonConvert.DeserializeObject<ChessBoard>(serializedChessBoard);

            Assert.AreEqual(chessBoard, deserializedChessBoard);
        }
    }
}
