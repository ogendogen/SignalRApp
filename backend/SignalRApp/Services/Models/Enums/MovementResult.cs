using System.ComponentModel.DataAnnotations;

namespace SignalRApp.Services.Models.Enums;

public enum MovementResult
{
    X,
    Circle,
    NotAllowed,
    Player1Wins,
    Player2Wins,
    Draw,
    Error,
}
