namespace Abb.Euopc.SharedDesks.Domain.Entities;

public sealed class Desk : Entity
{
    public string Label { get; set; } = default!;

    public string? ImageUrl { get; set; }

    public int? AreaId { get; set; }

    public Area? Area { get; set; }

    public List<DeskItemToDesk> DeskItemsToDesks { get; set; } = new List<DeskItemToDesk>();



    public bool HasImage => !string.IsNullOrEmpty(ImageUrl);

    public int ReservationsCount { get; init; }

    public int ItemsCount { get; init; }

    public Reservation? Reservation { get; init; }

    public override string ToString()
    {
        return $"Id: {Id} | Label: {Label} | Image path: {ImageUrl} | Area ID: {AreaId}";
    }
}
