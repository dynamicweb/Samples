using Dynamicweb.Extensibility.Mapping;

namespace ExpressDelivery.Models;

public sealed class ExpressDeliveryPresetMappingConfiguration : MappingConfigurationBase
{
    public ExpressDeliveryPresetMappingConfiguration()
    {
        CreateMap<Api.ExpressDeliveryPreset, ExpressDeliveryPresetDataModel>()
            .BindMapper((delivery, model) =>
            {
                model.TimeLimitInHours = delivery.Hours;
            });
    }
}
