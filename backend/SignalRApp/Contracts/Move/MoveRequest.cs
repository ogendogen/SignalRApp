using SignalRApp.Services.Models.Enums;

namespace SignalRApp.Contracts.Move;

public record MoveRequest(Guid GameId, string PlayerName, int X, int Y, FieldStatus FieldStatus);