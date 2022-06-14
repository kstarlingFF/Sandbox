using GithubActions.Persistance.Sql;

namespace GithubActions;
public class DBWorker : IWorker<int>
{
    private readonly IPersonRepository _personRepo;

    public DBWorker(IPersonRepository personRepo)
    {
        this._personRepo = personRepo;
    }

    public async Task<string> GetDataAsync(int key)
    {
        var person = await this._personRepo.FindAsync(key);

        if (person == null)
        {
            return null;
        }

        return person.Name;
    }
}
