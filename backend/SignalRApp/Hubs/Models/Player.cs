namespace SignalRApp.Hubs.Models;

public class Player : IEquatable<Player?>
{
    public required string ConnectionId { get; set; }
    public required string Name { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Player);
    }

    public bool Equals(Player? other)
    {
        return other is not null &&
               ConnectionId == other.ConnectionId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ConnectionId);
    }

    public static bool operator ==(Player? left, Player? right)
    {
        return EqualityComparer<Player>.Default.Equals(left, right);
    }

    public static bool operator !=(Player? left, Player? right)
    {
        return !(left == right);
    }
}
