using SignalRApp.Hubs.Models;

namespace SignalRApp.Services.Interfaces;

public interface IGroupsService
{
    string FindPlayersGroup(string playerName);
    void AddGroup(string groupName, Player player1, Player player2);
    void DeleteGroup(string groupName);
}
