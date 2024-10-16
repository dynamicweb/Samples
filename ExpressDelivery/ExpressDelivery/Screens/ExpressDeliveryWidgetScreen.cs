using Dynamicweb.CoreUI;
using Dynamicweb.CoreUI.Screens;
using ExpressDelivery.Api;
using ExpressDelivery.Components;
using ExpressDelivery.Models;

namespace ExpressDelivery.Screens;

public sealed class ExpressDeliveryWidgetScreen : ScreenBase<ExpressDeliveryDecoratedOrderDataModel>
{
    protected override UiComponentBase GetDefinitionInternal()
    {
        if (Model?.Order is null || Model.DeliveryPreset is null)
            throw new InvalidOperationException();

        var deliveryInfo = Helper.GetDeliveryInfo(Model.Order, Model.DeliveryPreset);

        return new ExpressDeliveryWidget()
        {
            Header = $"Express delivery - {Model.DeliveryPreset.Name}",
            ShippingComment = deliveryInfo.Text,
            ShippingLimit = Model.DeliveryPreset.Hours,
            RemainingTime = deliveryInfo.RemainingTime,
        };
    }
}
