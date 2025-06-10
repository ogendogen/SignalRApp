using SignalRApp.Services.Interfaces;

namespace SignalRApp.Services;

public class GroupsService : IGroupsService
{
    private readonly List<string> _groups = [];
    public void DeleteGroup(string groupName)
    {
        _groups.Remove(groupName);
    }

    public string FindPlayersGroup(string playerName)
    {
        return _groups.FirstOrDefault(g => g.Contains(playerName))!;
    }

    public void SaveGroup(string groupName)
    {
        _groups.Add(groupName);
    }
}
