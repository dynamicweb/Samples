using Dynamicweb.Application.UI;
using Dynamicweb.CoreUI.Navigation;
using ExpressDelivery.Models;

namespace ExpressDelivery.Tree;

public sealed class ExpressDeliveryNavigationNodePathProvider : NavigationNodePathProvider<ExpressDeliveryPresetDataModel>
{
    public ExpressDeliveryNavigationNodePathProvider()
    {
        AllowNullModel = true;
    }

    protected override NavigationNodePath GetNavigationNodePathInternal(ExpressDeliveryPresetDataModel? model) =>
        new([typeof(SettingsArea).FullName, NavigationContext.Empty, typeof(AreasSection).FullName, "ExpressDelivery"]);
}
