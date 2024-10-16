using Dynamicweb.CoreUI.Data;
using Dynamicweb.Ecommerce;
using ExpressDelivery.Api;

namespace ExpressDelivery.Commands;

public sealed class OrderStatusCheckCommand : CommandBase
{
    public string? OrderId { get; set; }
    public long? PresetId { get; set; }

    public override CommandResult Handle()
    {
        var preset = ExpressDeliveryPresetService.GetExpressDeliveryById(PresetId ?? 0);
        var order = Services.Orders.GetById(OrderId);
        var isElapsed = Helper.GetDeliveryInfo(order, preset).IsElapsed;

        return new()
        {
            Status = CommandResult.ResultType.Ok,
            Model = !isElapsed
        };
    }
}
