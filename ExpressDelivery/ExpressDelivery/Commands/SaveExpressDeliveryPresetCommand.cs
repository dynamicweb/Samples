using Dynamicweb.CoreUI.Data;
using ExpressDelivery.Api;
using ExpressDelivery.Models;

namespace ExpressDelivery.Commands;

public sealed class SaveExpressDeliveryPresetCommand : CommandBase<ExpressDeliveryPresetDataModel>
{
    public override CommandResult Handle()
    {
        if (Model is null) return new()
        {
            Status = CommandResult.ResultType.Invalid,
            Message = "Model data must be given"
        };

        var isNew = Model.Id == 0;
        var preset = isNew
            ? new()
            : ExpressDeliveryPresetService.GetExpressDeliveryById(Model!.Id) ?? new();

        preset.Name = Model.Name;
        preset.Hours = Model.TimeLimitInHours;
        preset.UnderHalfWayText = Model.UnderHalfWayText;
        preset.OverHalfWayText = Model.OverHalfWayText;
        preset.TooLateText = Model.TooLateText;

        if (!ExpressDeliveryPresetService.Save(preset)) return new()
        {
            Status = CommandResult.ResultType.Error,
            Message = "An unknown error occurred while saving the preset"
        };

        return new()
        {
            Status = CommandResult.ResultType.Ok,
            Model = Model
        };
    }
}
