using JsMate.Application.Interfaces;
using JsMate.Domain.Entities;
using JsMate.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace JsMate.Application.Services;

/// <summary>
/// Implementation of chess game operations.
/// </summary>
public class ChessService : IChessService
{
    private readonly IBoardRepository _boardRepository;
    private readonly ILogger<ChessService> _logger;

    public ChessService(IBoardRepository boardRepository, ILogger<ChessService> logger)
    {
        _boardRepository = boardRepository;
        _logger = logger;
    }

    public async Task<ChessBoard> GetOrCreateBoardAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var board = await _boardRepository.GetByIdAsync(id, cancellationToken);

        if (board != null)
        {
            _logger.LogDebug("Found existing board {BoardId}", id);
            return board;
        }

        _logger.LogInformation("Creating new board {BoardId}", id);
        board = ChessBoard.CreateWithInitialSetup(id);
        await _boardRepository.CreateAsync(board, cancellationToken);

        return board;
    }

    public async Task<IReadOnlyList<BoardPosition>> GetValidMovesAsync(
        Guid boardId,
        BoardPosition position,
        CancellationToken cancellationToken = default)
    {
        var board = await _boardRepository.GetByIdAsync(boardId, cancellationToken);

        if (board == null)
        {
            _logger.LogWarning("Board {BoardId} not found when getting valid moves", boardId);
            return [];
        }

        var piece = board.GetPieceAt(position);
        if (piece == null)
        {
            _logger.LogDebug("No piece at position {Position} on board {BoardId}", position, boardId);
            return [];
        }

        var legalMoves = board.GetLegalMoves(position);
        _logger.LogDebug("Found {MoveCount} legal moves for {PieceType} at {Position}",
            legalMoves.Count, piece.Type, position);

        return legalMoves;
    }

    public async Task<MoveResult> ExecuteMoveAsync(
        Guid boardId,
        BoardPosition from,
        BoardPosition to,
        CancellationToken cancellationToken = default)
    {
        var board = await _boardRepository.GetByIdAsync(boardId, cancellationToken);

        if (board == null)
        {
            _logger.LogWarning("Board {BoardId} not found when executing move", boardId);
            return MoveResult.Failure("Board not found.");
        }

        var result = board.ExecuteMove(from, to);

        if (result.IsSuccess)
        {
            await _boardRepository.UpdateAsync(board, cancellationToken);
            _logger.LogInformation("Move executed on board {BoardId}: {Move}", boardId, result.ExecutedMove);
        }
        else
        {
            _logger.LogDebug("Move rejected on board {BoardId}: {Error}", boardId, result.ErrorMessage);
        }

        return result;
    }

    public async Task<bool> UndoMoveAsync(Guid boardId, CancellationToken cancellationToken = default)
    {
        var board = await _boardRepository.GetByIdAsync(boardId, cancellationToken);

        if (board == null)
        {
            _logger.LogWarning("Board {BoardId} not found when undoing move", boardId);
            return false;
        }

        if (!board.CanUndo)
        {
            _logger.LogDebug("No moves to undo on board {BoardId}", boardId);
            return false;
        }

        var success = board.UndoMove();

        if (success)
        {
            await _boardRepository.UpdateAsync(board, cancellationToken);
            _logger.LogInformation("Move undone on board {BoardId}", boardId);
        }

        return success;
    }
}
