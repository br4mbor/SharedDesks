namespace Abb.Euopc.SharedDesks.Domain.Entities;

public sealed class DeskItemToDesk : Entity
{
    public int DeskItemId { get; set; }

    public DeskItem DeskItem { get; set; } = default!;

    public int DeskId { get; set; }

    public Desk Desk { get; set; } = default!;
}
