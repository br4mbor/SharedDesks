namespace Abb.Euopc.SharedDesks.Domain.Entities;

public sealed class Area : Entity
{
    public string Name { get; set; } = default!;

    public int Floor { get; set; }

    public string? ImageUrl { get; set; }

    public override string ToString()
    {
        return $"Id: {Id} | Name: {Name} | Floor: {Floor}";
    }
}
