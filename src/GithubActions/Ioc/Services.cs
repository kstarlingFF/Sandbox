using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubActions.Persistance;
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
        services.AddScoped<IWorker, Worker>();
    }
}
