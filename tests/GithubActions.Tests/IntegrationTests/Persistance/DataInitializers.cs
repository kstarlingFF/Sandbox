using GithubActions.Persistance;
using Microsoft.EntityFrameworkCore;

namespace GithubActions.Tests.IntegrationTests.Persistance;

public class DataInitializers
{
    public static void BuildPersistanceData(string connectionString)
    {
        var builder = new DbContextOptionsBuilder<SandboxContext>();
        builder.UseSqlServer(connectionString);
        var options = builder.Options;

        var ctx = new SandboxContext(options);
        ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();

        var person = new Person
        {
            Id = 2,
            Name = "Elrond"
        };

        using (var transaction = ctx.Database.BeginTransaction())
        {
            ctx.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[Person] ON");
            ctx.Person.Add(person);
            ctx.SaveChanges();
            transaction.Commit();
        }
    }
}
