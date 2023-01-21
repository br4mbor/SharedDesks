namespace Abb.Euopc.SharedDesks.Domain.Entities;

public sealed class Reservation : Entity
{
    public string CreatedByEmail { get; set; } = default!;

    public string ReservedForEmail { get; set; } = default!;

    public DateTime Date { get; set; }

    public int DeskId { get; set; }

    public Desk Desk { get; set; } = default!;

    public override string ToString()
    {
        return $"Id: {Id} | DeskId: {DeskId} | CreatedByUser Email: {CreatedByEmail} | ReservedForUser Email: {ReservedForEmail} | Reservation date: {Date}";
    }
}
