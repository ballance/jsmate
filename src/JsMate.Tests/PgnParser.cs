using System.Text.RegularExpressions;
using JsMate.Domain.ValueObjects;
using JsMate.Domain.Entities;
using JsMate.Domain.Enums;

namespace JsMate.Tests;

/// <summary>
/// Parses PGN (Portable Game Notation) chess games into moves.
/// </summary>
public static class PgnParser
{
    /// <summary>
    /// Parses a PGN move string and returns the from/to positions.
    /// </summary>
    public static (BoardPosition From, BoardPosition To)? ParseMove(string pgnMove, ChessBoard board)
    {
        // Remove check/checkmate indicators and annotations
        pgnMove = pgnMove.Trim()
            .Replace("+", "")
            .Replace("#", "")
            .Replace("!", "")
            .Replace("?", "");

        // Handle castling
        if (pgnMove == "O-O" || pgnMove == "0-0")
        {
            return HandleKingsideCastling(board);
        }
        if (pgnMove == "O-O-O" || pgnMove == "0-0-0")
        {
            return HandleQueensideCastling(board);
        }

        // Parse standard moves
        return ParseStandardMove(pgnMove, board);
    }

    private static (BoardPosition From, BoardPosition To)? HandleKingsideCastling(ChessBoard board)
    {
        var backRank = board.CurrentTurn == PieceTeam.White ? 7 : 0;
        var from = new BoardPosition(backRank, 4); // King's starting position
        var to = new BoardPosition(backRank, 6);   // Kingside castling destination
        return (from, to);
    }

    private static (BoardPosition From, BoardPosition To)? HandleQueensideCastling(ChessBoard board)
    {
        var backRank = board.CurrentTurn == PieceTeam.White ? 7 : 0;
        var from = new BoardPosition(backRank, 4); // King's starting position
        var to = new BoardPosition(backRank, 2);   // Queenside castling destination
        return (from, to);
    }

    private static (BoardPosition From, BoardPosition To)? ParseStandardMove(string pgnMove, ChessBoard board)
    {
        // Handle pawn promotion (e.g., e8=Q)
        PieceType? promotionPiece = null;
        if (pgnMove.Contains('='))
        {
            var parts = pgnMove.Split('=');
            pgnMove = parts[0];
            // promotionPiece = ParsePieceType(parts[1][0]); // For future promotion support
        }

        // Determine piece type
        var pieceType = PieceType.Pawn;
        var moveStart = 0;

        if (pgnMove.Length > 0 && char.IsUpper(pgnMove[0]) && pgnMove[0] != 'O')
        {
            pieceType = ParsePieceType(pgnMove[0]);
            moveStart = 1;
        }

        // Parse the rest of the move
        var moveStr = pgnMove[moveStart..];

        // Remove capture indicator
        var isCapture = moveStr.Contains('x');
        moveStr = moveStr.Replace("x", "");

        // The destination is always the last two characters
        if (moveStr.Length < 2)
            return null;

        var destStr = moveStr[^2..];
        var destCol = destStr[0] - 'a';
        var destRow = 8 - (destStr[1] - '0');

        if (!BoardPosition.IsValidCoordinate(destRow) || !BoardPosition.IsValidCoordinate(destCol))
            return null;

        var destination = new BoardPosition(destRow, destCol);

        // Parse disambiguation (file, rank, or both)
        char? disambigFile = null;
        char? disambigRank = null;

        if (moveStr.Length > 2)
        {
            var disambig = moveStr[..^2];
            foreach (var c in disambig)
            {
                if (c >= 'a' && c <= 'h')
                    disambigFile = c;
                else if (c >= '1' && c <= '8')
                    disambigRank = c;
            }
        }

        // Find the piece that can make this move
        var candidates = board.GetActivePieces(board.CurrentTurn)
            .Where(p => p.Type == pieceType)
            .ToList();

        foreach (var piece in candidates)
        {
            // Check disambiguation
            if (disambigFile.HasValue && (piece.Position.Col != disambigFile.Value - 'a'))
                continue;
            if (disambigRank.HasValue && (piece.Position.Row != 8 - (disambigRank.Value - '0')))
                continue;

            // Check if this piece can legally move to the destination
            var legalMoves = board.GetLegalMoves(piece.Position);
            if (legalMoves.Contains(destination))
            {
                return (piece.Position, destination);
            }
        }

        return null;
    }

    private static PieceType ParsePieceType(char c)
    {
        return c switch
        {
            'K' => PieceType.King,
            'Q' => PieceType.Queen,
            'R' => PieceType.Rook,
            'B' => PieceType.Bishop,
            'N' => PieceType.Knight,
            _ => PieceType.Pawn
        };
    }

    /// <summary>
    /// Extracts move list from PGN game text.
    /// </summary>
    public static List<string> ExtractMoves(string pgn)
    {
        var moves = new List<string>();

        // Remove comments
        pgn = Regex.Replace(pgn, @"\{[^}]*\}", "");
        pgn = Regex.Replace(pgn, @"\([^)]*\)", "");

        // Remove header tags
        pgn = Regex.Replace(pgn, @"\[[^\]]*\]", "");

        // Remove result
        pgn = Regex.Replace(pgn, @"1-0|0-1|1/2-1/2|\*", "");

        // Remove move numbers
        pgn = Regex.Replace(pgn, @"\d+\.\s*", " ");
        pgn = Regex.Replace(pgn, @"\d+\.\.\.", " ");

        // Split by whitespace and filter
        var tokens = pgn.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var token in tokens)
        {
            var cleaned = token.Trim();
            if (!string.IsNullOrEmpty(cleaned) &&
                cleaned != "..." &&
                !cleaned.All(char.IsDigit))
            {
                moves.Add(cleaned);
            }
        }

        return moves;
    }
}
