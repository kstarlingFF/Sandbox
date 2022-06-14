using GithubActions.Persistance.Storage;

namespace GithubActions;
public class StorageWorker : IWorker<string>
{
    private readonly IBlobStorage<string> _storage;

    public StorageWorker(IBlobStorage<string> storage)
    {
        this._storage = storage;
    }

    public async Task<string> GetDataAsync(string key)
    {
        var result = await this._storage.FindAsync(key);

        return result;
    }
}
