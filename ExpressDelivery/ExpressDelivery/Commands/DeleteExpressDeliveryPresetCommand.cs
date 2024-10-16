using Dynamicweb.CoreUI.Data;
using ExpressDelivery.Api;

namespace ExpressDelivery.Commands;

public sealed class DeleteExpressDeliveryPresetCommand : CommandBase
{
    public long Id { get; set; }

    public override CommandResult Handle() => ExpressDeliveryPresetService.Delete(Id)
        ? new() { Status = CommandResult.ResultType.Ok }
        : new() { Status = CommandResult.ResultType.Error, Message = "An unknown error occurred while trying to delete the preset" };
}
