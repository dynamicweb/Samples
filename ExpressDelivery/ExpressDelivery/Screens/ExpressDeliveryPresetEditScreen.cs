using Dynamicweb.CoreUI.Data;
using Dynamicweb.CoreUI.Screens;
using ExpressDelivery.Commands;
using ExpressDelivery.Models;

namespace ExpressDelivery.Screens;

public sealed class ExpressDeliveryPresetEditScreen : EditScreenBase<ExpressDeliveryPresetDataModel>
{
    protected override void BuildEditScreen()
    {
        AddComponents("General",
        [
            new("General",
            [
                EditorFor(m => m.Name),
                EditorFor(m => m.TimeLimitInHours),
            ]),
            new("Texts",
            [
                EditorFor(m => m.OverHalfWayText),
                EditorFor(m => m.UnderHalfWayText),
                EditorFor(m => m.TooLateText)
            ])
        ]);
    }

    protected override string GetScreenName() => "Edit Express Delivery Preset";
    protected override CommandBase<ExpressDeliveryPresetDataModel> GetSaveCommand() => new SaveExpressDeliveryPresetCommand();
}