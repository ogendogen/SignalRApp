using SignalRApp.Hubs.Models;
using SignalRApp.Services.Models.Enums;

namespace SignalRApp.Services.Interfaces;

public interface ITicTacToeService
{
    Guid StartGame(Player player1, Player player2);
    MovementResult Move(Guid gameId, Player player, int x, int y, FieldStatus fieldStatus);
    bool IsGameExists(Guid gameId);
}
