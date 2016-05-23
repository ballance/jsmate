using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsMate.Service.Models;
using NUnit.Framework;

namespace JsMate.Service.Test
{
    [TestFixture]
    public class BoardCreationTests
    {
        [Test]
        public void ShouldCreateDefaultChessBoard()
        {
            var startingBoard = new ChessBoard();
        }
    }
}
