using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Abb.Euopc.SharedDesks.WebClient.Pages.Administration;

public abstract class DetailDialogBase : ComponentBase
{
    [CascadingParameter]
    protected MudDialogInstance Dialog { get; set; } = default!;

    [Parameter]
    public int? Id { get; set; }
}

public abstract class DetailDialogBase<TEntity, TEntityService, TEntityValidator> : DetailDialogBase
    where TEntity : Entity, new()
    where TEntityService : IEntityService<TEntity>
    where TEntityValidator : IValidator<TEntity>
{
    protected TEntity _model = new();
    protected MudForm? _form;
    protected bool _formValid;

    [Inject]
    protected TEntityService Service { get; set; } = default!;

    [Inject]
    protected TEntityValidator Validator { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        if (Id is not null)
        {
            _model = await Service.GetByIdAsync(Id.Value) ?? new();
        }
    }

    protected virtual Task Save()
    {
        bool result;

        if (_model.Id == 0)
        {
            result = Service.Add(_model);
        }
        else
        {
            result = Service.Update(_model);
        }

        Dialog.Close(DialogResult.Ok(result));

        return Task.CompletedTask;
    }

    protected void Cancel()
    {
        Dialog.Cancel();
    }

}
