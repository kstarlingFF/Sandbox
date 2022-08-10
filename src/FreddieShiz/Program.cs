using System;
using Microsoft.Extensions.Hosting;
using FormFree.Plumbing.Storage.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using FreddieShiz;
using Microsoft.Extensions.Configuration;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(ctx =>
    {
        ctx.AddJsonFile("appsettings.json");
    })
    .ConfigureServices((ctx, services) => 
    {
        services.AddBlobStorage(options =>
        {
            options.ConnectionString = ctx.Configuration["AzureStorage:ConnectionString"];
            options.BlobContainerName = ctx.Configuration["AzureStorage:BlobContainerName"];
            options.CreateIfNotExists = false;
        });

        services.AddScoped<Worker>();
    })
    .Build();

var path = @"C:\workspace\DVOE\FreddieShiz\cte\cte.txt"; 

var vodList = (File.ReadAllLines(path)).ToList();
var worker = host.Services.GetRequiredService<Worker>();

await worker.Run(vodList);