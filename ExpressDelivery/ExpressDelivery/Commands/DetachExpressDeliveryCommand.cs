using Dynamicweb.CoreUI.Data;
using Dynamicweb.CoreUI.Data.Validation;
using ExpressDelivery.Api;

namespace ExpressDelivery.Commands;

public sealed class DetachExpressDeliveryCommand : CommandBase
{
    [Required]
    public string? OrderId { get; set; }

    public override CommandResult Handle()
    {
        if (string.IsNullOrEmpty(OrderId))
            return new()
            {
                Status = CommandResult.ResultType.Invalid,
                Message = "Order ID is required"
            };
        
        var didDetach = ExpressDeliveryPresetService.DetachPreset(OrderId!);

        return new()
        {
            Status = didDetach ? CommandResult.ResultType.Ok : CommandResult.ResultType.Error,
            Message = didDetach ? "Express delivery removed" : "Failed to remove express delivery"
        };
    }
}
