using FluentAssertions;
using JsMate.Domain.Entities;
using JsMate.Domain.Enums;
using JsMate.Domain.ValueObjects;

namespace JsMate.Tests.Domain;

public class ChessBoardTests
{
    [Fact]
    public void CreateWithInitialSetup_Creates32Pieces()
    {
        var board = ChessBoard.CreateWithInitialSetup();

        board.Pieces.Should().HaveCount(32);
    }

    [Fact]
    public void CreateWithInitialSetup_Creates16PiecesPerTeam()
    {
        var board = ChessBoard.CreateWithInitialSetup();

        board.GetActivePieces(PieceTeam.White).Should().HaveCount(16);
        board.GetActivePieces(PieceTeam.Black).Should().HaveCount(16);
    }

    [Fact]
    public void CreateWithInitialSetup_WhiteStartsFirst()
    {
        var board = ChessBoard.CreateWithInitialSetup();

        board.CurrentTurn.Should().Be(PieceTeam.White);
    }

    [Fact]
    public void CreateWithInitialSetup_PlacesPawnsCorrectly()
    {
        var board = ChessBoard.CreateWithInitialSetup();

        // White pawns on row 6
        for (var col = 0; col < 8; col++)
        {
            var piece = board.GetPieceAt(new BoardPosition(6, col));
            piece.Should().NotBeNull();
            piece!.Type.Should().Be(PieceType.Pawn);
            piece.Team.Should().Be(PieceTeam.White);
        }

        // Black pawns on row 1
        for (var col = 0; col < 8; col++)
        {
            var piece = board.GetPieceAt(new BoardPosition(1, col));
            piece.Should().NotBeNull();
            piece!.Type.Should().Be(PieceType.Pawn);
            piece.Team.Should().Be(PieceTeam.Black);
        }
    }

    [Fact]
    public void CreateWithInitialSetup_PlacesKingsCorrectly()
    {
        var board = ChessBoard.CreateWithInitialSetup();

        var whiteKing = board.GetPieceAt(new BoardPosition(7, 4));
        whiteKing.Should().NotBeNull();
        whiteKing!.Type.Should().Be(PieceType.King);
        whiteKing.Team.Should().Be(PieceTeam.White);

        var blackKing = board.GetPieceAt(new BoardPosition(0, 4));
        blackKing.Should().NotBeNull();
        blackKing!.Type.Should().Be(PieceType.King);
        blackKing.Team.Should().Be(PieceTeam.Black);
    }

    [Fact]
    public void GetPieceAt_EmptySquare_ReturnsNull()
    {
        var board = ChessBoard.CreateWithInitialSetup();

        var piece = board.GetPieceAt(new BoardPosition(4, 4));

        piece.Should().BeNull();
    }

    [Fact]
    public void IsEmpty_EmptySquare_ReturnsTrue()
    {
        var board = ChessBoard.CreateWithInitialSetup();

        board.IsEmpty(new BoardPosition(4, 4)).Should().BeTrue();
    }

    [Fact]
    public void IsEmpty_OccupiedSquare_ReturnsFalse()
    {
        var board = ChessBoard.CreateWithInitialSetup();

        board.IsEmpty(new BoardPosition(6, 0)).Should().BeFalse();
    }

    [Fact]
    public void ExecuteMove_ValidPawnMove_Succeeds()
    {
        var board = ChessBoard.CreateWithInitialSetup();
        var from = new BoardPosition(6, 4); // e2 white pawn
        var to = new BoardPosition(4, 4);   // e4

        var result = board.ExecuteMove(from, to);

        result.IsSuccess.Should().BeTrue();
        board.GetPieceAt(from).Should().BeNull();
        board.GetPieceAt(to).Should().NotBeNull();
        board.GetPieceAt(to)!.Type.Should().Be(PieceType.Pawn);
    }

    [Fact]
    public void ExecuteMove_ValidMove_SwitchesTurn()
    {
        var board = ChessBoard.CreateWithInitialSetup();
        board.CurrentTurn.Should().Be(PieceTeam.White);

        var result = board.ExecuteMove(new BoardPosition(6, 4), new BoardPosition(4, 4));

        result.IsSuccess.Should().BeTrue();
        board.CurrentTurn.Should().Be(PieceTeam.Black);
    }

    [Fact]
    public void ExecuteMove_WrongTeamTurn_Fails()
    {
        var board = ChessBoard.CreateWithInitialSetup();
        var from = new BoardPosition(1, 4); // Black pawn
        var to = new BoardPosition(3, 4);

        var result = board.ExecuteMove(from, to);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("turn");
    }

    [Fact]
    public void ExecuteMove_InvalidDestination_Fails()
    {
        var board = ChessBoard.CreateWithInitialSetup();
        var from = new BoardPosition(6, 4);
        var to = new BoardPosition(3, 4); // Too far for pawn

        var result = board.ExecuteMove(from, to);

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ExecuteMove_EmptySource_Fails()
    {
        var board = ChessBoard.CreateWithInitialSetup();
        var from = new BoardPosition(4, 4); // Empty
        var to = new BoardPosition(3, 4);

        var result = board.ExecuteMove(from, to);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("No piece");
    }
}
