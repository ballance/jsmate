using JsMate.Domain.Entities;

namespace JsMate.Application.Interfaces;

/// <summary>
/// Repository interface for chess board persistence.
/// </summary>
public interface IBoardRepository
{
    /// <summary>
    /// Gets a chess board by its ID.
    /// </summary>
    Task<ChessBoard?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new chess board.
    /// </summary>
    Task<ChessBoard> CreateAsync(ChessBoard board, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing chess board.
    /// </summary>
    Task<bool> UpdateAsync(ChessBoard board, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a chess board by its ID.
    /// </summary>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a chess board exists.
    /// </summary>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
