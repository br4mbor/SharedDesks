namespace Abb.Euopc.SharedDesks.Domain.Entities;

public sealed class DeskItemType : Entity
{
    public string Name { get; set; } = default!;

    public bool IsDeleted { get; set; }

    public override string ToString()
    {
        return $"Id: {Id} | Name: {Name} | IsDeleted: {IsDeleted}";
    }

    public override bool Equals(object? obj)
    {
        var other = obj as DeskItemType;

        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
