using SignalRApp.Hubs.Models;

namespace SignalRApp.Services.Models;

public class Group : IEquatable<Group?>
{
    public required Guid GroupId { get; init; }
    public required Player Player1 { get; init; }
    public required Player Player2 { get; init; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Group);
    }

    public bool Equals(Group? other)
    {
        return other is not null &&
               GroupId.Equals(other.GroupId);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GroupId);
    }

    public static bool operator ==(Group? left, Group? right)
    {
        return EqualityComparer<Group>.Default.Equals(left, right);
    }

    public static bool operator !=(Group? left, Group? right)
    {
        return !(left == right);
    }
}
