namespace ExpressDelivery.Api;

internal sealed class OrderDeliveryInfo
{
    public bool IsElapsed { get; set; } = true;
    public string Text { get; set; } = "";
    public TimeSpan RemainingTime { get; set; }
}
