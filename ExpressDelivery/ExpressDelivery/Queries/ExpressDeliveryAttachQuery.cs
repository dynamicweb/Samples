using Dynamicweb.CoreUI.Data;
using ExpressDelivery.Models;

namespace ExpressDelivery.Queries;

public sealed class ExpressDeliveryAttachQuery : DataQueryModelBase<ExpressDeliveryAttachModel>
{
    public long PresetId { get; set; }
    public string? OrderId { get; set; }

    public override ExpressDeliveryAttachModel? GetModel() => new()
    {
        PresetId = PresetId,
        OrderId = OrderId
    };
}
