using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using Abb.Euopc.SharedDesks.Domain.Validators;
using Abb.Euopc.SharedDesks.WebClient.Helpers;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Abb.Euopc.SharedDesks.WebClient.Pages.Administration.Areas
{
    public partial class AreaDetailDialog : DetailDialogBase<Area, IAreaService, AreaValidator>
    {
        private IBrowserFile? _areaImage;

        protected override async Task Save()
        {
            bool result;
            byte[]? image = null;

            if (_areaImage is not null)
            {
                image = await ImageHelper.ConvertImageAsync(_areaImage);
            }

            if (_model.Id == 0)
            {
                result = await Service.AddWithImageAsync(_model, image);
            }
            else
            {
                result = await Service.UpdateWithImageAsync(_model, image);
            }

            Dialog.Close(DialogResult.Ok(result));
        }
    }
}
