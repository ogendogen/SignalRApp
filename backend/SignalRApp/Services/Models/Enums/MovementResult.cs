using System.ComponentModel.DataAnnotations;

namespace SignalRApp.Services.Models.Enums;

public enum MovementResult
{
    Allowed,
    NotAllowed,
    Player1Wins,
    Player2Wins,
    Draw,
    GameNotFound
}
