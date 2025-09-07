using SignalRApp.Services.Models.Enums;

namespace SignalRApp.Contracts.Move;

public record MoveRequest(Guid GameId, int X, int Y);