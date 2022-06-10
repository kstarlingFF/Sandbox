using System.Threading.Tasks;
using GithubActions.Ioc;
using GithubActions.Tests.IntegrationTests.Persistance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace GithubActions.Tests.IntegrationTests;
internal class WorkerIntegrationTest
{
    private IWorker worker;

    [SetUp]
    public void Setup()
    {
        var configurations = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        DataInitializers.BuildPersistanceData(configurations["ConnectionString"]);

        var services = new ServiceCollection();
        ServicesExtension.AddServices(services, configurations);
        var provider = services.BuildServiceProvider();

        worker = provider.GetRequiredService<IWorker>();
    }

    [Test]
    public async Task TestWorker()
    {
        var name = await this.worker.GetNameAsync(2);

        Assert.That(name, Is.EqualTo("Elrond"));
    }
}
