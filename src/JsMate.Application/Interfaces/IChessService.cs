using JsMate.Domain.Entities;
using JsMate.Domain.ValueObjects;

namespace JsMate.Application.Interfaces;

/// <summary>
/// Service interface for chess game operations.
/// </summary>
public interface IChessService
{
    /// <summary>
    /// Gets or creates a chess board by ID.
    /// </summary>
    Task<ChessBoard> GetOrCreateBoardAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets valid moves for a piece at the given position.
    /// </summary>
    Task<IReadOnlyList<BoardPosition>> GetValidMovesAsync(Guid boardId, BoardPosition position, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a move on the board.
    /// </summary>
    Task<MoveResult> ExecuteMoveAsync(Guid boardId, BoardPosition from, BoardPosition to, CancellationToken cancellationToken = default);

    /// <summary>
    /// Undoes the last move on the board.
    /// </summary>
    Task<bool> UndoMoveAsync(Guid boardId, CancellationToken cancellationToken = default);
}
