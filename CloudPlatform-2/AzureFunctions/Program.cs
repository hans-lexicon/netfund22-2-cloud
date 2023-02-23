using AzureFunctions.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(x =>
    {
        x.AddDbContext<CosmosDbContext>(x => x.UseCosmos(Environment.GetEnvironmentVariable("CosmosDB")!, Environment.GetEnvironmentVariable("CosmosDBName")!));
    })
    .Build();

host.Run();
