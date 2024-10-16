using Dynamicweb.CoreUI.Actions;
using Dynamicweb.CoreUI.Actions.Implementations;
using Dynamicweb.CoreUI.Editors;
using Dynamicweb.CoreUI.Editors.Lists;
using Dynamicweb.CoreUI.Screens;
using ExpressDelivery.Api;
using ExpressDelivery.Commands;
using ExpressDelivery.Models;
using static Dynamicweb.CoreUI.Editors.Inputs.ListBase;

namespace ExpressDelivery.Screens;

public sealed class ExpressDeliverySelectPromptScreen : PromptScreenBase<ExpressDeliveryAttachModel>
{
    protected override void BuildPromptScreen()
    {
        var editor = EditorForCommand<AttachPresetToOrderCommand, long>(c => c.PresetId, "Select preset to attach");
        AddComponent(editor, "Preset");
    }

    protected override string GetScreenName() => "Attach preset";

    protected override ActionBase? GetOkAction() =>
        RunCommandAction
            .For<AttachPresetToOrderCommand>(new() { OrderId = Model?.OrderId })
            .WithOnSuccess(new CompositeAction(ClosePopupAction.Default, ReloadWorkspaceAction.Default));

    protected override EditorBase? GetEditorForCommand(string propertyName) => propertyName switch
    {
        nameof(AttachPresetToOrderCommand.PresetId) => GetPresetSelect(),
        _ => null
    };

    private static Select GetPresetSelect()
    {
        var presets = ExpressDeliveryPresetService.GetExpressDeliveries();
        return new Select
        {
            Options = presets.Select(p => new ListOption() { Label = p.Name ?? $"Preset ID: {p.Id}", Value = p.Id }).ToList()
        };
    }
}
