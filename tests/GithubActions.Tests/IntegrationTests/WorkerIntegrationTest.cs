using System.Threading.Tasks;
using GithubActions.Ioc;
using GithubActions.Tests.IntegrationTests.Persistance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace GithubActions.Tests.IntegrationTests;
[TestFixture]
internal class WorkerIntegrationTest
{
    private ServiceProvider provider;
    private IConfiguration configuration;

    [SetUp]
    public void Setup()
    {
        configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();


        var services = new ServiceCollection();
        ServicesExtension.AddServices(services, configuration);
        provider = services.BuildServiceProvider();
    }

    [Test]
    public async Task TestDbWorker()
    {
        DataInitializers.BuildPersistanceData(configuration["ConnectionString"]);

        var worker = this.provider.GetRequiredService<IWorker<int>>();
        
        var name = await worker.GetDataAsync(2);

        Assert.That(name, Is.EqualTo("Elrond"));
    }

    [Test]
    public async Task TestStorageWorker()
    {
        StorageInitializer.InitializeBlobStorage(configuration);

        var worker = this.provider.GetRequiredService<IWorker<string>>();
        var document = await worker.GetDataAsync("sandbox.json");

        Assert.NotNull(document);
        Assert.That(document, Is.EqualTo("this is a test"));
    }
}
