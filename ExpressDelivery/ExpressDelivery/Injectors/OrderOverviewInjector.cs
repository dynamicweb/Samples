using Dynamicweb.CoreUI;
using Dynamicweb.CoreUI.Actions;
using Dynamicweb.CoreUI.Actions.Implementations;
using Dynamicweb.CoreUI.Layout;
using Dynamicweb.CoreUI.Screens;
using Dynamicweb.Ecommerce.UI.Screens;
using ExpressDelivery.Api;
using ExpressDelivery.Commands;
using ExpressDelivery.Queries;
using ExpressDelivery.Screens;

namespace ExpressDelivery.Injectors;

public sealed class OrderOverviewInjector : ScreenInjector<OrderOverviewScreen>
{
    public override void OnAfter(OrderOverviewScreen screen, UiComponentBase content)
    {
        if (content.Get<ScreenLayout>() is not ScreenLayout layout) return;
        if (layout.Get<TabContainer>() is not TabContainer tabContainer) return;
        if (screen.Model?.Id is not string orderId) return;

        var existingPreset = ExpressDeliveryPresetService.GetExpressDeliveryPresetByOrderId(orderId);

        AddMenuItems(layout, orderId, existingPreset);
        AddWidget(tabContainer, orderId, existingPreset);
    }

    private static void AddWidget(TabContainer tabContainer, string orderId, ExpressDeliveryPreset? preset)
    {
        if (preset is null)
            return;

        var group = new Group().WithComponent(new Widget()
        {
            Component = new Dynamicweb.CoreUI.Displays.ShowScreen()
            {
                Load = Dynamicweb.CoreUI.Displays.ShowScreen.LoadMethod.Inline,
                Query = new ExpressDeliveryPresetByOrderIdQuery() { OrderId = orderId },
                Value = typeof(ExpressDeliveryWidgetScreen),
                IsRefreshable = true,
                RefreshIntervalInMs = 1000,
                ContinueRefreshCommand = new OrderStatusCheckCommand() { OrderId = orderId, PresetId = preset.Id },
                AutoStart = true,
            },
            Label = preset.Name ?? "Express delivery",
            Icon = Dynamicweb.CoreUI.Icons.Icon.FastMail
        });

        group.Width = Group.GroupWidth.Col_6;

        var tab = tabContainer.GetOrAddTab("Express delivery");
        tab.Section.WithGroup(group);
    }

    private static void AddMenuItems(ScreenLayout layout, string orderId, ExpressDeliveryPreset? preset)
    {
        List<ActionNode> nodes = [];

        nodes.Add(new()
        {
            Name = $"{(preset is null ? "Add" : "Change")} express delivery preset",
            Icon = Dynamicweb.CoreUI.Icons.Icon.LinkAdd,
            NodeAction = OpenDialogAction
                .To<ExpressDeliverySelectPromptScreen>()
                .With(new ExpressDeliveryAttachQuery() { OrderId = orderId })
        });

        if (preset is not null)
        {
            nodes.Add(new()
            {
                Name = "Detach express delivery preset",
                Icon = Dynamicweb.CoreUI.Icons.Icon.LinkBroken,
                NodeAction = ConfirmAction.For(
                    RunCommandAction.For(new DetachExpressDeliveryCommand() { OrderId = orderId }).WithReloadWorkspaceOnSuccess(),
                    $"Detach express delivery preset",
                    $"Are you sure you want to detach the express delivery preset (preset id: {preset.Id})?"
                )
            });
        }

        layout.ContextActionGroups.Add(new() { Nodes = nodes });
    }
}
