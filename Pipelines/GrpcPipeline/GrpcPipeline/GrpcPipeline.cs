using Dynamicweb.Host.Core;
using GrpcPipeline.Services;

namespace GrpcPipeline;

public sealed class GrpcPipeline : IPipeline
{
    public int Rank { get; }

    public void RegisterApplicationComponents(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(options =>
        {
            options.MapGrpcService<GreeterService>();
        });
    }

    public void RegisterServices(IServiceCollection services, IMvcCoreBuilder mvcBuilder)
    {
        services.AddGrpc();
    }

    public void RunInitializers()
    {

    }
}
