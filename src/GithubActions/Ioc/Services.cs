using System.Data.SqlClient;
using Azure.Storage.Blobs;
using GithubActions.Persistance.Sql;
using GithubActions.Persistance.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GithubActions.Ioc;
public class ServicesExtension
{
    public static void AddServices(IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IDatabaseConnection>(ctx =>
        {
            return new DatabaseConnection(new SqlConnection(configuration["ConnectionString"]));
        });
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IWorker<int>, DBWorker>();
        services.AddScoped<IWorker<string>, StorageWorker>();

        services.AddScoped<IBlobStorage<string>>(ctx =>
        {
            var serviceClient = new BlobServiceClient(configuration["Storage:StorageConnectionString"]);
            var client = serviceClient.GetBlobContainerClient(configuration["Storage:Container"]);
            client.CreateIfNotExists();
            return new BlobStorage(client);
        });
    }
}
