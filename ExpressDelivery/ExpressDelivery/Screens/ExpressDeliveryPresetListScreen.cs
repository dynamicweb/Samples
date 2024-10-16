using Dynamicweb.Application.UI.Helpers;
using Dynamicweb.CoreUI.Actions;
using Dynamicweb.CoreUI.Actions.Implementations;
using Dynamicweb.CoreUI.Icons;
using Dynamicweb.CoreUI.Lists;
using Dynamicweb.CoreUI.Lists.ViewMappings;
using Dynamicweb.CoreUI.Screens;
using ExpressDelivery.Commands;
using ExpressDelivery.Models;
using ExpressDelivery.Queries;

namespace ExpressDelivery.Screens;

public sealed class ExpressDeliveryPresetListScreen : ListScreenBase<ExpressDeliveryPresetDataModel>
{
    protected override string GetScreenName() => "Express Delivery presets";

    protected override IEnumerable<ListViewMapping> GetViewMappings() =>
    [
        new RowViewMapping
        {
            Columns =
           [
                CreateMapping(m => m.Name),
                CreateMapping(m => m.TimeLimitInHours),
                CreateMapping(m => m.UnderHalfWayText),
                CreateMapping(m => m.OverHalfWayText),
                CreateMapping(m => m.TooLateText),
            ]
        }
    ];

    protected override ActionBase GetListItemPrimaryAction(ExpressDeliveryPresetDataModel model) =>
        NavigateScreenAction.To<ExpressDeliveryPresetEditScreen>().With(new ExpressDeliveryByIdQuery { Id = model.Id });

    protected override IEnumerable<ActionGroup>? GetListItemContextActions(ExpressDeliveryPresetDataModel model) =>
    [
        new()
        {
            Nodes = 
            [
                ActionBuilder.Edit<ExpressDeliveryPresetEditScreen>(new ExpressDeliveryByIdQuery { Id = model.Id }),
                ActionBuilder.Delete(new DeleteExpressDeliveryPresetCommand { Id = model.Id }, "Delete?", $"Are you sure you want to delete the preset? (ID: {model.Id})")
            ]
        }
    ];

    protected override ActionNode GetItemCreateAction() =>
        new() { Name = "New preset", Icon = Icon.PlusSquare, NodeAction = NavigateScreenAction.To<ExpressDeliveryPresetEditScreen>() };
}
