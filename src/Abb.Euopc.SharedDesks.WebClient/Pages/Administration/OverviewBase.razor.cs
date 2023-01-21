using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Abb.Euopc.SharedDesks.WebClient.Pages.Administration;

public partial class OverviewBase<TEntity, TEntityService, TDetailDialogType> : ComponentBase
    where TEntity : Entity, new()
    where TEntityService : IEntityService<TEntity>
    where TDetailDialogType : DetailDialogBase
{
    private MudMessageBox? _deleteMessageBox;
    protected MudTable<TEntity>? _table;

    protected virtual string EntityName => "Entity";

    [Inject]
    protected ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    protected TEntityService Service { get; set; } = default!;

    protected virtual async Task<TableData<TEntity>> LoadDataAsync(TableState state)
    {
        var data = await Service.GetAllAsync();
        var totalItems = data.Count;

        return new TableData<TEntity>() { TotalItems = totalItems, Items = data };
    }

    protected Task Add()
        => ShowDetail("Add", $"{EntityName} added", $"Error while adding {EntityName}", null);

    protected Task Edit(int id)
        => ShowDetail("Edit", $"{EntityName} updated", $"Error while updating {EntityName}", id);

    private async Task ShowDetail(string action, string successMessage, string errorMessage, int? id)
    {
        var dialog = DialogService.Show<TDetailDialogType>($"{action} {EntityName}", new DialogParameters { ["id"] = id }, new DialogOptions
        {
            CloseButton = false,
            CloseOnEscapeKey = false,
            DisableBackdropClick = true,
            FullWidth = true
        });
        var result = await dialog.Result;

        if (result.Cancelled)
        {
            return;
        }

        _ = bool.TryParse(result.Data.ToString(), out var success);

        if (!success)
        {
            Snackbar.Add(errorMessage, Severity.Error);
            return;
        }

        Snackbar.Add(successMessage, Severity.Success);

        await RefreshTable();
    }

    protected async Task Delete(TEntity entity)
    {
        if (_deleteMessageBox is null)
        {
            return;
        }

        var result = await _deleteMessageBox.Show();

        if (result is null)
        {
            return;
        }

        Service.Delete(entity);
        Snackbar.Add($"{EntityName} deleted", Severity.Success);

        await RefreshTable();
    }

    private async Task RefreshTable()
    {
        if (_table is not null)
        {
            await _table.ReloadServerData();
        }
    }
}
