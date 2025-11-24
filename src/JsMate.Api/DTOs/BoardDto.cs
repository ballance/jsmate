using JsMate.Domain.Entities;
using JsMate.Domain.Enums;

namespace JsMate.Api.DTOs;

public record BoardDto
{
    public Guid Id { get; init; }
    public string CurrentTurn { get; init; } = "White";
    public List<PieceDto> Pieces { get; init; } = [];

    public static BoardDto FromDomain(ChessBoard board)
    {
        return new BoardDto
        {
            Id = board.Id,
            CurrentTurn = board.CurrentTurn.ToString(),
            Pieces = board.Pieces
                .Where(p => p.IsActive)
                .Select(PieceDto.FromDomain)
                .ToList()
        };
    }
}

public record PieceDto
{
    public string PieceType { get; init; } = "";
    public string PieceTeam { get; init; } = "";
    public int Row { get; init; }
    public int Col { get; init; }
    public int PieceNumber { get; init; }
    public bool Active { get; init; }

    public static PieceDto FromDomain(ChessPiece piece)
    {
        return new PieceDto
        {
            PieceType = piece.Type.ToString(),
            PieceTeam = piece.Team == Domain.Enums.PieceTeam.White ? "White" : "Black",
            Row = piece.Position.Row,
            Col = piece.Position.Col,
            PieceNumber = piece.PieceNumber,
            Active = piece.IsActive
        };
    }
}

public record PositionDto(int Row, int Col);

public record MoveRequestDto(int FromRow, int FromCol, int ToRow, int ToCol);

public record MoveResultDto(bool Success, string? Message, MoveDto? Move);

public record MoveDto(PositionDto From, PositionDto To, bool IsCapture);
