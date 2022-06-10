using GithubActions.Persistance;
using Microsoft.EntityFrameworkCore;

namespace GithubActions.Tests.IntegrationTests.Persistance;

public class SandboxContext : DbContext
{
    public SandboxContext(DbContextOptions<SandboxContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Person> Person { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Person>()
            .ToTable("Person");
            
    }

    public override int SaveChanges()
    {
        this.ChangeTracker.DetectChanges();
        return base.SaveChanges();
    }
}
