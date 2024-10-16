namespace ExpressDelivery.Api;

public sealed class ExpressDeliveryPreset
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public int Hours { get; set; }
    public string? UnderHalfWayText { get;set; }
    public string? OverHalfWayText { get; set; }
    public string? TooLateText { get; set; }
}
