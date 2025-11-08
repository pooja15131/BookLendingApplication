using Amazon.Lambda.AspNetCoreServer;
using System.Diagnostics.CodeAnalysis;

namespace BookLendingApplication;

[ExcludeFromCodeCoverage]
public class LambdaEntryPoint : APIGatewayProxyFunction
{
    protected override void Init(IWebHostBuilder builder)
    {
        builder.UseContentRoot(Directory.GetCurrentDirectory())
               .UseStartup<Startup>();
    }
}