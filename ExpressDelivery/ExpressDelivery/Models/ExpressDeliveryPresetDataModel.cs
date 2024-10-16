using Dynamicweb.CoreUI.Data;

namespace ExpressDelivery.Models;

public sealed class ExpressDeliveryPresetDataModel : DataViewModelBase, IIdentifiable
{
    public long Id { get; set; }

    [ConfigurableProperty("Name")]
    public string? Name { get; set; }

    [ConfigurableProperty("Time limit (in hours)")]
    public int TimeLimitInHours { get; set; }

    [ConfigurableProperty("Message (under half way)")]
    public string? UnderHalfWayText { get; set; }

    [ConfigurableProperty("Message (over half way)")]
    public string? OverHalfWayText { get; set; }

    [ConfigurableProperty("Message (too late)")]
    public string? TooLateText { get; set; }

    public string GetId() => $"{Id}";
}
