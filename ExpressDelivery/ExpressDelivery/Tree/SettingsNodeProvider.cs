using Dynamicweb.Application.UI;
using Dynamicweb.CoreUI.Actions.Implementations;
using Dynamicweb.CoreUI.Navigation;
using ExpressDelivery.Queries;
using ExpressDelivery.Screens;

namespace ExpressDelivery.Tree;

public sealed class SettingsNodeProvider : NavigationNodeProvider<AreasSection>
{
    public override IEnumerable<NavigationNode> GetRootNodes()
    {
        yield return new()
        {
            Id = "ExpressDelivery",
            Name = "Express Delivery",
            NodeAction = NavigateScreenAction.To<ExpressDeliveryPresetListScreen>().With(new ExpressDeliveriesQuery()),
            HasSubNodes = false
        };
    }

    public override IEnumerable<NavigationNode> GetSubNodes(NavigationNodePath parentNodePath)
    {
        yield break;
    }
}
