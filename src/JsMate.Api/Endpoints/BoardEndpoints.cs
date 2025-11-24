using JsMate.Api.DTOs;
using JsMate.Application.Interfaces;
using JsMate.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace JsMate.Api.Endpoints;

public static class BoardEndpoints
{
    public static void MapBoardEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/board")
            .WithTags("Board")
            .WithOpenApi();

        // GET /api/board - Create a new board
        group.MapGet("/", async (IChessService chessService) =>
        {
            var boardId = Guid.NewGuid();
            var board = await chessService.GetOrCreateBoardAsync(boardId);
            return Results.Ok(BoardDto.FromDomain(board));
        })
        .WithName("CreateBoard")
        .WithSummary("Creates a new chess board with initial piece setup");

        // GET /api/board/{id} - Get or create a board by ID
        group.MapGet("/{id:guid}", async (Guid id, IChessService chessService) =>
        {
            var board = await chessService.GetOrCreateBoardAsync(id);
            return Results.Ok(BoardDto.FromDomain(board));
        })
        .WithName("GetBoard")
        .WithSummary("Gets a chess board by ID, or creates a new one if it doesn't exist");

        // GET /api/board/{id}/moves/{row}/{col} - Get valid moves for a piece
        group.MapGet("/{id:guid}/moves/{row:int}/{col:int}", async (
            Guid id,
            int row,
            int col,
            IChessService chessService) =>
        {
            try
            {
                var position = new BoardPosition(row, col);
                var validMoves = await chessService.GetValidMovesAsync(id, position);

                return Results.Ok(validMoves.Select(m => new PositionDto(m.Row, m.Col)));
            }
            catch (ArgumentOutOfRangeException)
            {
                return Results.BadRequest("Invalid board position");
            }
        })
        .WithName("GetValidMoves")
        .WithSummary("Gets valid moves for the piece at the specified position");

        // POST /api/board/{id}/move - Execute a move
        group.MapPost("/{id:guid}/move", async (
            Guid id,
            [FromBody] MoveRequestDto request,
            IChessService chessService) =>
        {
            try
            {
                var from = new BoardPosition(request.FromRow, request.FromCol);
                var to = new BoardPosition(request.ToRow, request.ToCol);

                var result = await chessService.ExecuteMoveAsync(id, from, to);

                if (result.IsSuccess)
                {
                    var move = result.ExecutedMove!.Value;
                    return Results.Ok(new MoveResultDto(
                        true,
                        null,
                        new MoveDto(
                            new PositionDto(move.From.Row, move.From.Col),
                            new PositionDto(move.To.Row, move.To.Col),
                            move.IsCapture)));
                }

                return Results.BadRequest(new MoveResultDto(false, result.ErrorMessage, null));
            }
            catch (ArgumentOutOfRangeException)
            {
                return Results.BadRequest(new MoveResultDto(false, "Invalid board position", null));
            }
        })
        .WithName("ExecuteMove")
        .WithSummary("Executes a move on the chess board");

        // POST /api/board/{id}/undo - Undo the last move
        group.MapPost("/{id:guid}/undo", async (
            Guid id,
            IChessService chessService) =>
        {
            var success = await chessService.UndoMoveAsync(id);

            if (success)
            {
                return Results.Ok(new { Success = true, Message = "Move undone successfully" });
            }

            return Results.BadRequest(new { Success = false, Message = "No moves to undo" });
        })
        .WithName("UndoMove")
        .WithSummary("Undoes the last move on the chess board");
    }
}
