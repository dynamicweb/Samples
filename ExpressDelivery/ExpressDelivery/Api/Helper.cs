using Dynamicweb.Ecommerce.Orders;

namespace ExpressDelivery.Api;

internal static class Helper
{
    public static OrderDeliveryInfo GetDeliveryInfo(Order? order, ExpressDeliveryPreset? preset)
    {
        if (order is null || preset is null || !order.CompletedDate.HasValue)
            return new();

        var spanBetween = DateTime.Now.Subtract(order.CompletedDate.Value);
        var allowedHours = Math.Abs(preset.Hours);
        var elapsedFraction = spanBetween.TotalHours / allowedHours;
        var text = elapsedFraction switch
        {
            < 0.5 => preset.OverHalfWayText,
            > 1 => preset.TooLateText,
            _ => preset.UnderHalfWayText
        };

        return new()
        {
            IsElapsed = elapsedFraction > 1,
            Text = text ?? "",
            RemainingTime = TimeSpan.FromHours(allowedHours) - spanBetween
        };
    }
}
