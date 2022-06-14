using System.Threading.Tasks;
using GithubActions.Persistance.Sql;
using Moq;
using NUnit.Framework;

namespace GithubActions.Tests.UnitTests;

public class WorkerTests
{
    private Mock<IPersonRepository> personRepoMock;

    [SetUp]
    public void Setup()
    {
        var mockrepo = new MockRepository(MockBehavior.Strict);
        personRepoMock = mockrepo.Create<IPersonRepository>();
    }

    [Test]
    public async Task TestWorker()
    {
        var id = 1;

        var person = new Person
        {
            Id = id,
            Name = "Aragorn"
        };

        personRepoMock
            .Setup(p => p.FindAsync(id))
            .ReturnsAsync(person);

        var worker = new DBWorker(personRepoMock.Object);
        var result = await worker.GetDataAsync(id);

        Assert.That(result, Is.EqualTo(person.Name));
    }
}