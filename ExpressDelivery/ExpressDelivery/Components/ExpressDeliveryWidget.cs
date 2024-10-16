using Dynamicweb.CoreUI;

namespace ExpressDelivery.Components;

public sealed class ExpressDeliveryWidget : UiComponentBase
{
    public string Header { get; set; } = "";
    public int ShippingLimit { get; set; }
    public string ShippingComment { get; set; } = "";
    public TimeSpan RemainingTime { get; internal set; }
}
