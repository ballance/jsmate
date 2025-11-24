using JsMate.Domain.Enums;

namespace JsMate.Domain.ValueObjects;

/// <summary>
/// Represents a chess move from one position to another.
/// </summary>
public readonly record struct Move
{
    public BoardPosition From { get; }
    public BoardPosition To { get; }
    public bool IsCapture { get; }
    public PieceType? PromotionPiece { get; }

    public Move(BoardPosition from, BoardPosition to, bool isCapture = false, PieceType? promotionPiece = null)
    {
        From = from;
        To = to;
        IsCapture = isCapture;
        PromotionPiece = promotionPiece;
    }

    /// <summary>
    /// Returns standard algebraic notation for the move.
    /// </summary>
    public string ToNotation() => IsCapture
        ? $"{From.ToAlgebraic()}x{To.ToAlgebraic()}"
        : $"{From.ToAlgebraic()}-{To.ToAlgebraic()}";

    public override string ToString() => ToNotation();
}
