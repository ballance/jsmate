namespace JsMate.Domain.ValueObjects;

/// <summary>
/// Represents a position on the chess board using standard 0-7 coordinates.
/// Row 0 is the top (Black's back rank), Row 7 is the bottom (White's back rank).
/// Col 0 is the left (a-file), Col 7 is the right (h-file).
/// </summary>
public readonly record struct BoardPosition
{
    public int Row { get; }
    public int Col { get; }

    public BoardPosition(int row, int col)
    {
        if (!IsValidCoordinate(row) || !IsValidCoordinate(col))
        {
            throw new ArgumentOutOfRangeException(
                $"Position ({row}, {col}) is outside the valid board range (0-7).");
        }

        Row = row;
        Col = col;
    }

    /// <summary>
    /// Creates a BoardPosition without validation. Use only when you're certain the coordinates are valid.
    /// </summary>
    public static BoardPosition CreateUnchecked(int row, int col) => new(row, col);

    /// <summary>
    /// Tries to create a BoardPosition, returning null if coordinates are invalid.
    /// </summary>
    public static BoardPosition? TryCreate(int row, int col)
    {
        if (!IsValidCoordinate(row) || !IsValidCoordinate(col))
            return null;

        return new BoardPosition(row, col);
    }

    public static bool IsValidCoordinate(int value) => value >= 0 && value <= 7;

    /// <summary>
    /// Returns the algebraic notation for this position (e.g., "e4", "a1").
    /// </summary>
    public string ToAlgebraic() => $"{(char)('a' + Col)}{8 - Row}";

    /// <summary>
    /// Creates a BoardPosition from algebraic notation (e.g., "e4", "a1").
    /// </summary>
    public static BoardPosition FromAlgebraic(string notation)
    {
        if (string.IsNullOrEmpty(notation) || notation.Length != 2)
            throw new ArgumentException("Invalid algebraic notation", nameof(notation));

        var col = char.ToLower(notation[0]) - 'a';
        var row = 8 - (notation[1] - '0');

        return new BoardPosition(row, col);
    }

    public override string ToString() => $"({Row}, {Col}) [{ToAlgebraic()}]";
}
