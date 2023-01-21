namespace Abb.Euopc.SharedDesks.Domain.Entities;

public sealed class DeskItem : Entity
{
    public string Name { get; set; } = default!;

    public bool IsDeleted { get; set; } = false;

    public int TypeId { get; set; }

    public DeskItemType Type { get; set; } = default!;

    public override string ToString()
    {
        return $"Id: {Id} | Name: {Name} | IsDeleted: {IsDeleted} | Type ID: {TypeId}";
    }
}
