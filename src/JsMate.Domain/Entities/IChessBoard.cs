using JsMate.Domain.Enums;
using JsMate.Domain.ValueObjects;

namespace JsMate.Domain.Entities;

/// <summary>
/// Interface for chess board operations needed by pieces for move calculation.
/// </summary>
public interface IChessBoard
{
    /// <summary>
    /// Gets the piece at the specified position, or null if empty.
    /// </summary>
    ChessPiece? GetPieceAt(BoardPosition position);

    /// <summary>
    /// Gets all active pieces on the board.
    /// </summary>
    IReadOnlyList<ChessPiece> GetActivePieces();

    /// <summary>
    /// Gets all active pieces for a specific team.
    /// </summary>
    IReadOnlyList<ChessPiece> GetActivePieces(PieceTeam team);

    /// <summary>
    /// Checks if a position is empty.
    /// </summary>
    bool IsEmpty(BoardPosition position);

    /// <summary>
    /// Checks if the given team's king is in check.
    /// </summary>
    bool IsInCheck(PieceTeam team);

    /// <summary>
    /// Gets the en passant target square, if any.
    /// This is the square that can be captured via en passant on the current turn.
    /// </summary>
    BoardPosition? EnPassantTarget { get; }

    /// <summary>
    /// Checks if the given team is in checkmate.
    /// </summary>
    bool IsCheckmate(PieceTeam team);

    /// <summary>
    /// Checks if the given team is in stalemate.
    /// </summary>
    bool IsStalemate(PieceTeam team);

    /// <summary>
    /// Gets legal moves for the piece at the given position (moves that don't leave king in check).
    /// </summary>
    IReadOnlyList<BoardPosition> GetLegalMoves(BoardPosition position);

    /// <summary>
    /// Gets the current game state.
    /// </summary>
    GameState GetGameState();
}
