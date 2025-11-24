using FluentAssertions;
using JsMate.Domain.ValueObjects;

namespace JsMate.Tests.Domain;

public class BoardPositionTests
{
    [Fact]
    public void Constructor_WithValidCoordinates_CreatesPosition()
    {
        var position = new BoardPosition(3, 4);

        position.Row.Should().Be(3);
        position.Col.Should().Be(4);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(8, 0)]
    [InlineData(0, 8)]
    [InlineData(-1, -1)]
    [InlineData(8, 8)]
    public void Constructor_WithInvalidCoordinates_ThrowsException(int row, int col)
    {
        var action = () => new BoardPosition(row, col);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void TryCreate_WithValidCoordinates_ReturnsPosition()
    {
        var position = BoardPosition.TryCreate(3, 4);

        position.Should().NotBeNull();
        position!.Value.Row.Should().Be(3);
        position.Value.Col.Should().Be(4);
    }

    [Fact]
    public void TryCreate_WithInvalidCoordinates_ReturnsNull()
    {
        var position = BoardPosition.TryCreate(-1, 0);

        position.Should().BeNull();
    }

    [Theory]
    [InlineData(0, 0, "a8")]
    [InlineData(0, 7, "h8")]
    [InlineData(7, 0, "a1")]
    [InlineData(7, 7, "h1")]
    [InlineData(4, 4, "e4")]
    [InlineData(6, 4, "e2")]
    public void ToAlgebraic_ReturnsCorrectNotation(int row, int col, string expected)
    {
        var position = new BoardPosition(row, col);

        position.ToAlgebraic().Should().Be(expected);
    }

    [Theory]
    [InlineData("a8", 0, 0)]
    [InlineData("h8", 0, 7)]
    [InlineData("a1", 7, 0)]
    [InlineData("h1", 7, 7)]
    [InlineData("e4", 4, 4)]
    [InlineData("e2", 6, 4)]
    public void FromAlgebraic_ReturnsCorrectPosition(string notation, int expectedRow, int expectedCol)
    {
        var position = BoardPosition.FromAlgebraic(notation);

        position.Row.Should().Be(expectedRow);
        position.Col.Should().Be(expectedCol);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(7, true)]
    [InlineData(4, true)]
    [InlineData(-1, false)]
    [InlineData(8, false)]
    public void IsValidCoordinate_ReturnsCorrectResult(int value, bool expected)
    {
        BoardPosition.IsValidCoordinate(value).Should().Be(expected);
    }

    [Fact]
    public void Equality_SameCoordinates_AreEqual()
    {
        var pos1 = new BoardPosition(3, 4);
        var pos2 = new BoardPosition(3, 4);

        pos1.Should().Be(pos2);
        (pos1 == pos2).Should().BeTrue();
    }

    [Fact]
    public void Equality_DifferentCoordinates_AreNotEqual()
    {
        var pos1 = new BoardPosition(3, 4);
        var pos2 = new BoardPosition(4, 3);

        pos1.Should().NotBe(pos2);
        (pos1 != pos2).Should().BeTrue();
    }
}
