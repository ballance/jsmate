using JsMate.Application.Interfaces;
using JsMate.Domain.Entities;
using JsMate.Infrastructure.Persistence.Models;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace JsMate.Infrastructure.Persistence;

/// <summary>
/// LiteDB implementation of the board repository.
/// </summary>
public class LiteDbBoardRepository : IBoardRepository, IDisposable
{
    private readonly LiteDatabase _database;
    private readonly ILiteCollection<ChessBoardDocument> _collection;
    private readonly ILogger<LiteDbBoardRepository> _logger;
    private bool _disposed;

    public LiteDbBoardRepository(string connectionString, ILogger<LiteDbBoardRepository> logger)
    {
        _database = new LiteDatabase(connectionString);
        _collection = _database.GetCollection<ChessBoardDocument>("chessBoards");
        _collection.EnsureIndex(x => x.Id, unique: true);
        _logger = logger;
    }

    public Task<ChessBoard?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var document = _collection.FindOne(x => x.Id == id);
            var board = document?.ToDomain();

            if (board != null)
            {
                _logger.LogDebug("Retrieved board {BoardId} from database", id);
            }

            return Task.FromResult(board);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve board {BoardId}", id);
            return Task.FromResult<ChessBoard?>(null);
        }
    }

    public Task<ChessBoard> CreateAsync(ChessBoard board, CancellationToken cancellationToken = default)
    {
        var document = ChessBoardDocument.FromDomain(board);
        _collection.Insert(document);
        _logger.LogInformation("Created board {BoardId}", board.Id);
        return Task.FromResult(board);
    }

    public Task<bool> UpdateAsync(ChessBoard board, CancellationToken cancellationToken = default)
    {
        try
        {
            var document = ChessBoardDocument.FromDomain(board);
            var result = _collection.Update(document);

            if (result)
            {
                _logger.LogDebug("Updated board {BoardId}", board.Id);
            }
            else
            {
                _logger.LogWarning("Board {BoardId} not found for update", board.Id);
            }

            return Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update board {BoardId}", board.Id);
            return Task.FromResult(false);
        }
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = _collection.DeleteMany(x => x.Id == id) > 0;

            if (result)
            {
                _logger.LogInformation("Deleted board {BoardId}", id);
            }

            return Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete board {BoardId}", id);
            return Task.FromResult(false);
        }
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exists = _collection.Exists(x => x.Id == id);
        return Task.FromResult(exists);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _database.Dispose();
            _disposed = true;
        }
    }
}
