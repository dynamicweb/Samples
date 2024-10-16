using Dynamicweb.CoreUI.Data;
using Dynamicweb.Extensibility.Mapping;
using ExpressDelivery.Api;
using ExpressDelivery.Models;

namespace ExpressDelivery.Queries;

public sealed class ExpressDeliveryByIdQuery : DataQueryModelBase<ExpressDeliveryPresetDataModel>
{
    public long Id { get; set; }

    public override ExpressDeliveryPresetDataModel? GetModel()
    {
        var delivery = ExpressDeliveryPresetService.GetExpressDeliveryById(Id);
        return MappingService.Map<ExpressDeliveryPresetDataModel>(delivery);
    }
}
