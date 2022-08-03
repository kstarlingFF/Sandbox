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

List<string> IqaDvoeIds = new List<string> {
                "F6128F71-6CAA-476A-A050-0EC497E0B724.freddie.json",
                "7254FBF1-73DD-4ECB-82E5-1E37569C2D9A.freddie.json",
                "41D2959F-C493-4612-ADEE-62C3C44EF2ED.freddie.json",
                "5A78E0F8-2765-48EF-8949-657D442C383A.freddie.json",
                "13D2DA94-74C4-4392-A21C-73123695B957.freddie.json",
                "1A468485-2B71-47EC-B07E-73E63F92578A.freddie.json",
                "1B831881-DFE2-4344-96B3-A7B4C45E1EDB.freddie.json",
                "841A166B-6D0F-48B3-997E-B0CD57F1093D.freddie.json",
                "ECE328B3-BFA3-427C-9DC6-CDEA623137EC.freddie.json",
                "D1F0C4F4-D1A0-4BA9-BD2D-D05580C85935.freddie.json" };

var worker = host.Services.GetRequiredService<Worker>();
await worker.Run(IqaDvoeIds);