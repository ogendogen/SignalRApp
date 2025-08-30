using SignalRApp.Hubs.Models;
using SignalRApp.Services.Models;

namespace SignalRApp.Services.Interfaces;

public interface IGroupsService
{
    void AddGroup(Guid gameId, Player player1, Player player2);
    void DeleteGroupByAnyPlayer(Player player);
    Player GetPlayerByPlayerName(string playerName);
    Player GetSecondPlayerByFirstPlayer(Player player);
    Group GetGroupByAnyPlayerName(string playerName);
    Group? GetGroupByGameId(Guid gameId);
}
