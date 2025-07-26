using SignalRApp.Services.Models.Enums;

namespace SignalRApp.Services.Interfaces;

public interface ITicTacToeService
{
    Guid StartGame(string player1, string player2);
    MovementResult Move(string groupName, string player, int x, int y, FieldStatus fieldStatus);
    void EndGame(string player1, string player2);
}
