using GithubActions.Persistance;

namespace GithubActions;
public class Worker : IWorker
{
    private readonly IPersonRepository _personRepo;

    public Worker(IPersonRepository personRepo)
    {
        this._personRepo = personRepo;
    }

    public async Task<string> GetNameAsync(int id)
    {
        var person = await this._personRepo.FindAsync(id);

        if (person == null)
        {
            return null;
        }

        return person.Name;
    }
}
