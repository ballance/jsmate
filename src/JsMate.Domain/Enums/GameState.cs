namespace JsMate.Domain.Enums;

/// <summary>
/// Represents the current state of the chess game.
/// </summary>
public enum GameState
{
    /// <summary>
    /// Game is in progress, no special conditions.
    /// </summary>
    InProgress,

    /// <summary>
    /// Current player's king is in check but can escape.
    /// </summary>
    Check,

    /// <summary>
    /// Current player's king is in checkmate - game over.
    /// </summary>
    Checkmate,

    /// <summary>
    /// Current player has no legal moves but is not in check - draw.
    /// </summary>
    Stalemate
}
