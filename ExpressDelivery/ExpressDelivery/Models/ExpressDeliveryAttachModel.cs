using Dynamicweb.CoreUI.Data;

namespace ExpressDelivery.Models;

public sealed class ExpressDeliveryAttachModel : DataViewModelBase
{
    public long PresetId { get; set; }
    public string? OrderId { get; set; }
}
