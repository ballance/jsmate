using FluentAssertions;
using JsMate.Domain.Entities;
using JsMate.Domain.Entities.Pieces;
using JsMate.Domain.Enums;
using JsMate.Domain.ValueObjects;

namespace JsMate.Tests.Domain;

public class PieceMovementTests
{
    [Fact]
    public void Pawn_GetValidMoves_FromStartPosition_CanMoveTwoSquares()
    {
        var board = ChessBoard.CreateWithInitialSetup();
        var pawn = board.GetPieceAt(new BoardPosition(6, 4))!; // e2 white pawn

        var moves = pawn.GetValidMoves(board);

        moves.Should().Contain(new BoardPosition(5, 4)); // e3
        moves.Should().Contain(new BoardPosition(4, 4)); // e4
    }

    [Fact]
    public void Pawn_GetValidMoves_BlockedByPiece_CannotMoveForward()
    {
        var board = new ChessBoard();
        var whitePawn = new Pawn(PieceTeam.White, new BoardPosition(4, 4), 1);
        var blockingPawn = new Pawn(PieceTeam.Black, new BoardPosition(3, 4), 1);
        board.AddPiece(whitePawn);
        board.AddPiece(blockingPawn);

        var moves = whitePawn.GetValidMoves(board);

        moves.Should().NotContain(new BoardPosition(3, 4));
    }

    [Fact]
    public void Pawn_GetValidMoves_CanCaptureDiagonally()
    {
        var board = new ChessBoard();
        var whitePawn = new Pawn(PieceTeam.White, new BoardPosition(4, 4), 1);
        var blackPawn = new Pawn(PieceTeam.Black, new BoardPosition(3, 5), 1);
        board.AddPiece(whitePawn);
        board.AddPiece(blackPawn);

        var moves = whitePawn.GetValidMoves(board);

        moves.Should().Contain(new BoardPosition(3, 5)); // Diagonal capture
        moves.Should().Contain(new BoardPosition(3, 4)); // Forward move
    }

    [Fact]
    public void Knight_GetValidMoves_FromCenter_Has8Moves()
    {
        var board = new ChessBoard();
        var knight = new Knight(PieceTeam.White, new BoardPosition(4, 4), 1);
        board.AddPiece(knight);

        var moves = knight.GetValidMoves(board);

        moves.Should().HaveCount(8);
        moves.Should().Contain(new BoardPosition(2, 3));
        moves.Should().Contain(new BoardPosition(2, 5));
        moves.Should().Contain(new BoardPosition(3, 2));
        moves.Should().Contain(new BoardPosition(3, 6));
        moves.Should().Contain(new BoardPosition(5, 2));
        moves.Should().Contain(new BoardPosition(5, 6));
        moves.Should().Contain(new BoardPosition(6, 3));
        moves.Should().Contain(new BoardPosition(6, 5));
    }

    [Fact]
    public void Knight_GetValidMoves_FromCorner_HasLimitedMoves()
    {
        var board = new ChessBoard();
        var knight = new Knight(PieceTeam.White, new BoardPosition(0, 0), 1);
        board.AddPiece(knight);

        var moves = knight.GetValidMoves(board);

        moves.Should().HaveCount(2);
        moves.Should().Contain(new BoardPosition(2, 1));
        moves.Should().Contain(new BoardPosition(1, 2));
    }

    [Fact]
    public void Knight_CanJumpOverPieces()
    {
        var board = ChessBoard.CreateWithInitialSetup();
        var knight = board.GetPieceAt(new BoardPosition(7, 1))!; // b1 knight

        var moves = knight.GetValidMoves(board);

        // Knight can jump to c3 and a3 despite pawns in the way
        moves.Should().Contain(new BoardPosition(5, 0)); // a3
        moves.Should().Contain(new BoardPosition(5, 2)); // c3
    }

    [Fact]
    public void Rook_GetValidMoves_CanMoveOrthogonally()
    {
        var board = new ChessBoard();
        var rook = new Rook(PieceTeam.White, new BoardPosition(4, 4), 1);
        board.AddPiece(rook);

        var moves = rook.GetValidMoves(board);

        // Should be able to move to all squares in same row and column
        for (var i = 0; i < 8; i++)
        {
            if (i != 4)
            {
                moves.Should().Contain(new BoardPosition(i, 4)); // Vertical
                moves.Should().Contain(new BoardPosition(4, i)); // Horizontal
            }
        }
    }

    [Fact]
    public void Rook_GetValidMoves_StopsAtBlockingPiece()
    {
        var board = new ChessBoard();
        var rook = new Rook(PieceTeam.White, new BoardPosition(4, 4), 1);
        var blocker = new Pawn(PieceTeam.White, new BoardPosition(4, 6), 1);
        board.AddPiece(rook);
        board.AddPiece(blocker);

        var moves = rook.GetValidMoves(board);

        moves.Should().Contain(new BoardPosition(4, 5)); // One square before blocker
        moves.Should().NotContain(new BoardPosition(4, 6)); // Blocker position
        moves.Should().NotContain(new BoardPosition(4, 7)); // Beyond blocker
    }

    [Fact]
    public void Bishop_GetValidMoves_CanMoveDiagonally()
    {
        var board = new ChessBoard();
        var bishop = new Bishop(PieceTeam.White, new BoardPosition(4, 4), 1);
        board.AddPiece(bishop);

        var moves = bishop.GetValidMoves(board);

        // Should include diagonal moves
        moves.Should().Contain(new BoardPosition(3, 3));
        moves.Should().Contain(new BoardPosition(2, 2));
        moves.Should().Contain(new BoardPosition(3, 5));
        moves.Should().Contain(new BoardPosition(5, 3));
        moves.Should().Contain(new BoardPosition(5, 5));
    }

    [Fact]
    public void Queen_GetValidMoves_CombinesRookAndBishop()
    {
        var board = new ChessBoard();
        var queen = new Queen(PieceTeam.White, new BoardPosition(4, 4), 1);
        board.AddPiece(queen);

        var moves = queen.GetValidMoves(board);

        // Orthogonal moves (like rook)
        moves.Should().Contain(new BoardPosition(4, 0));
        moves.Should().Contain(new BoardPosition(0, 4));

        // Diagonal moves (like bishop)
        moves.Should().Contain(new BoardPosition(0, 0));
        moves.Should().Contain(new BoardPosition(7, 7));
    }

    [Fact]
    public void King_GetValidMoves_MovesOneSquareInAnyDirection()
    {
        var board = new ChessBoard();
        var king = new King(PieceTeam.White, new BoardPosition(4, 4), 1);
        board.AddPiece(king);

        var moves = king.GetValidMoves(board);

        moves.Should().HaveCount(8);
        moves.Should().Contain(new BoardPosition(3, 3));
        moves.Should().Contain(new BoardPosition(3, 4));
        moves.Should().Contain(new BoardPosition(3, 5));
        moves.Should().Contain(new BoardPosition(4, 3));
        moves.Should().Contain(new BoardPosition(4, 5));
        moves.Should().Contain(new BoardPosition(5, 3));
        moves.Should().Contain(new BoardPosition(5, 4));
        moves.Should().Contain(new BoardPosition(5, 5));
    }
}
