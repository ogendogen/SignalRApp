using SignalRApp.Services.Models.Enums;

namespace SignalRApp.Contracts.Move;

public class MoveResponse
{
    public string Error { get; init; } = string.Empty;
    public bool IsSuccess => string.IsNullOrEmpty(Error) && MovementResult != MovementResult.Error;
    public required MovementResult MovementResult { get; init; }
    public string PlayerName { get; init; } = string.Empty;
}