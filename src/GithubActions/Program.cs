using GithubActions;
using GithubActions.Ioc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var configurations = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => 
        ServicesExtension.AddServices(services, configurations))
    .Build();

var worker = host.Services.GetRequiredService<IWorker<int>>();

var name = worker.GetDataAsync(1).Result;

Console.WriteLine(name);
