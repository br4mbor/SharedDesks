using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;
using Abb.Euopc.SharedDesks.Domain.Requests;
using Abb.Euopc.SharedDesks.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace Abb.Euopc.SharedDesks.EF.Repositories;

internal sealed class DeskRepository : Repository<Desk>, IDeskRepository
{
    public DeskRepository(SharedDesksContext context) : base(context)
    { }

    public override IQueryable<Desk> GetAll()
    {
        return Get()
            .Include(d => d.Area)
            .Select(d => new Desk
            {
                Id = d.Id,
                Label = d.Label,
                Area = d.Area,
                AreaId = d.AreaId,
                ImageUrl = d.ImageUrl,
                ItemsCount = d.DeskItemsToDesks.Count,
                ReservationsCount = _context.Reservations.Where(r => r.DeskId == d.Id && r.Date >= DateTime.Today).Count()
            })
            .OrderBy(d => d.Label);
    }

    public override Task<List<Desk>> GetAllAsync()
    {
        return GetAll()
            .ToListAsync();
    }

    public async Task<(List<Desk>, int)> GetAllPagedAsync(int page, int pageSize, string? searchString = null)
    {
        var query = GetAll();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(d => d.Label.Contains(searchString) || d.Area!.Name.Contains(searchString));
        }

        return (await query
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(), await query.CountAsync());
    }

    public override Desk? GetById(int id)
    {
        return Get(d => d.Id == id)
            .Include(d => d.Area)
            .Include(d => d.DeskItemsToDesks)
            .ThenInclude(ditd => ditd.DeskItem)
            .ThenInclude(di => di.Type)
            .SingleOrDefault();
    }

    public override Task<Desk?> GetByIdAsync(int id)
    {
        return Get(d => d.Id == id)
            .Include(d => d.Area)
            .Include(d => d.DeskItemsToDesks)
            .ThenInclude(ditd => ditd.DeskItem)
            .ThenInclude(di => di.Type)
            .SingleOrDefaultAsync();
    }

    public override Desk Add(Desk desk)
    {
        desk.DeskItemsToDesks.ForEach(ditd =>
        {
            ditd.Desk = null!;
            ditd.DeskItem = null!;
        });

        desk.AreaId = desk.Area?.Id;
        desk.Area = null;

        return base.Add(desk);
    }

    public override Desk Update(Desk desk)
    {
        var deskToUpdate = GetById(desk.Id);

        if (deskToUpdate is null)
        {
            throw new DbUpdateException($"No desk found with Id: {desk.Id} to update.");
        }

        var itemsToAdd = desk.DeskItemsToDesks.Where(ditd => ditd.Id == 0).Select(ditd => new DeskItemToDesk() { DeskId = ditd.DeskId, DeskItemId = ditd.DeskItemId }).ToArray();
        deskToUpdate.DeskItemsToDesks.RemoveAll(ditd => !desk.DeskItemsToDesks.Select(d => d.Id).Contains(ditd.Id));
        deskToUpdate.DeskItemsToDesks.AddRange(itemsToAdd);
        deskToUpdate.AreaId = desk.Area?.Id;
        //deskToUpdate.Area = desk.Area;
        deskToUpdate.ImageUrl = desk.ImageUrl;
        deskToUpdate.Label = desk.Label;

        return base.Update(deskToUpdate);
    }

    public Task<List<Desk>> GetByFilterAsync(DeskFilterRequest deskFilter)
    {
        var query = Get();

        if (deskFilter.SelectedDates?.Length > 0)
            query = FilterByDate(query, deskFilter.SelectedDates);

        if (deskFilter.SelectedFloors?.Length > 0)
            query = FilterByFloor(query, deskFilter.SelectedFloors);

        if (deskFilter.SelectedAreaIds?.Length > 0)
            query = FilterByArea(query, deskFilter.SelectedAreaIds);

        if (deskFilter.SelectedItemIds?.Length > 0)
            query = FilterByItems(query, deskFilter.SelectedItemIds);

        return query.Include(d => d.Area)
            .OrderBy(d => d.Label)
            .ToListAsync();
    }

    public Task<List<Desk>> GetAvailableDesksAsync(DateTime date, int areaId)
    {
        return Get(d => d.AreaId == areaId)
            .Select(d => new Desk
            {
                Id = d.Id,
                Label = d.Label,
                Area = d.Area,
                AreaId = d.AreaId,
                ImageUrl = d.ImageUrl,
                Reservation = _context.Reservations.FirstOrDefault(r => r.DeskId == d.Id && r.Date == date)
            })
            .OrderBy(d => d.Label)
            .ToListAsync();
    }

    public async Task<bool> IsUniqueLabelAsync(int deskId, string label)
    {
        return !(await Get(d => d.Id != deskId && d.Label == label)
            .AnyAsync());
    }

    private IQueryable<Desk> FilterByDate(IQueryable<Desk> desks, DateTime[] notReservedDates)
    {
        return desks.Where(d => !_context.Reservations.Where(r => r.DeskId == d.Id && notReservedDates.Contains(r.Date)).Any());
    }

    private static IQueryable<Desk> FilterByFloor(IQueryable<Desk> desks, int[] floors)
    {
        return desks.Where(d => floors.Contains(d.Area!.Floor));
    }

    private static IQueryable<Desk> FilterByArea(IQueryable<Desk> desks, int[] areaIds)
    {
        return desks.Where(d => areaIds.Contains(d.Area!.Id));
    }

    private static IQueryable<Desk> FilterByItems(IQueryable<Desk> desks, int[] requiredItemIds)
    {
        return desks.Where(d => d.DeskItemsToDesks.Where(ditd => requiredItemIds.Contains(ditd.DeskItemId))
            .Select(ditd => new { ditd.DeskItemId })
            .Distinct()
            .Count() == requiredItemIds.Length);
    }
}
