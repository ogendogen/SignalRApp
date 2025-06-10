namespace SignalRApp.Services.Interfaces;

public interface IGroupsService
{
    string FindPlayersGroup(string playerName);
    void SaveGroup(string groupName);
    void DeleteGroup(string groupName);
}
