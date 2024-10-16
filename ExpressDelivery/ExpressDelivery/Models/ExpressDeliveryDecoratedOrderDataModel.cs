using Dynamicweb.CoreUI.Data;
using Dynamicweb.Ecommerce.Orders;
using ExpressDelivery.Api;

namespace ExpressDelivery.Models;

public sealed class ExpressDeliveryDecoratedOrderDataModel : DataViewModelBase
{
    public Order? Order { get; set; }
    public ExpressDeliveryPreset? DeliveryPreset { get; set; }
}
