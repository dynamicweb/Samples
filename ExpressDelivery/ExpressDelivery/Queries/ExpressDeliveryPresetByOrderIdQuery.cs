using Dynamicweb.CoreUI.Data;
using ExpressDelivery.Api;
using ExpressDelivery.Models;

namespace ExpressDelivery.Queries;

public sealed class ExpressDeliveryPresetByOrderIdQuery : DataQueryModelBase<ExpressDeliveryDecoratedOrderDataModel>
{
    public string? OrderId { get; set; }
    public override ExpressDeliveryDecoratedOrderDataModel GetModel()
    {
        if (OrderId is null)
            return new();

        var preset = ExpressDeliveryPresetService.GetExpressDeliveryPresetByOrderId(OrderId);
        if (preset is null)
            return new();

        var order = Dynamicweb.Ecommerce.Services.Orders.GetById(OrderId);

        return new()
        {
            Order = order,
            DeliveryPreset = preset
        };
    }
}
