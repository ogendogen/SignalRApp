using SignalRApp.Hubs.Models;
using SignalRApp.Services.Interfaces;
using SignalRApp.Services.Models;

namespace SignalRApp.Services;

public class GroupsService : IGroupsService
{
    private readonly HashSet<Group> _groups = [];
    public void DeleteGroupByAnyPlayer(Player player)
    {
        var group = _groups.First(g => g.Player1 == player || g.Player2 == player);
        _groups.Remove(group);
    }

    public void AddGroup(Guid gameId, Player player1, Player player2)
    {
        var group = new Group() { GroupId = gameId, Player1  = player1, Player2 = player2 };
        _groups.Add(group);
    }

    public Player GetSecondPlayerByFirstPlayer(Player player)
    {
        var group = _groups.First(g => g.Player1 == player || g.Player2 == player);
        return group.Player1 == player ? group.Player2 : group.Player1;
    }

    public Player GetPlayerByPlayerName(string playerName)
    {
        var group = _groups.First(g => g.Player1.Name == playerName || g.Player2.Name == playerName);
        return group.Player1.Name == playerName ? group.Player1 : group.Player2;
    }

    public Group GetGroupByAnyPlayerName(string playerName)
    {
        return _groups.First(g => g.Player1.Name == playerName || g.Player2.Name == playerName);
    }

    public Group? GetGroupByGameId(Guid gameId)
    {
        return _groups.FirstOrDefault(g => g.GroupId == gameId);
    }
}
