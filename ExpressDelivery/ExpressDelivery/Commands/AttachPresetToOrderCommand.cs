using Dynamicweb.CoreUI.Data;
using ExpressDelivery.Api;

namespace ExpressDelivery.Commands;

public sealed class AttachPresetToOrderCommand : CommandBase
{
    public long PresetId { get; set; }
    public string? OrderId { get; set; }

    public override CommandResult Handle()
    {
        if (OrderId is not null && PresetId > 0)
            return ExpressDeliveryPresetService.AttachOrUpdatePreset(OrderId, PresetId)
                ? new() { Status = CommandResult.ResultType.Ok }
                : new() { Status = CommandResult.ResultType.Error, Message = "An unknown error occurred while trying to attach the preset to the order" };
        return new() { Status = CommandResult.ResultType.Invalid, Message = $"A valid {nameof(OrderId)} and {nameof(PresetId)} is required" };
    }
}
