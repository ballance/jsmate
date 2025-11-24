using JsMate.Domain.Enums;

namespace JsMate.Domain.ValueObjects;

/// <summary>
/// Records all information needed to undo a move.
/// </summary>
public record MoveRecord
{
    public BoardPosition From { get; init; }
    public BoardPosition To { get; init; }
    public PieceType MovedPieceType { get; init; }
    public PieceTeam MovedPieceTeam { get; init; }
    public int MovedPieceNumber { get; init; }

    // Capture info
    public PieceType? CapturedPieceType { get; init; }
    public PieceTeam? CapturedPieceTeam { get; init; }
    public int? CapturedPieceNumber { get; init; }
    public BoardPosition? CapturedPiecePosition { get; init; } // For en passant, differs from To

    // Special move info
    public bool WasCastling { get; init; }
    public BoardPosition? RookFrom { get; init; }
    public BoardPosition? RookTo { get; init; }

    // State before move
    public BoardPosition? PreviousEnPassantTarget { get; init; }
    public bool PieceHadMoved { get; init; } // For tracking HasMoved on King/Rook/Pawn
}
