namespace GithubActions;

public interface IWorker<TKey>
{
    Task<string> GetDataAsync(TKey key);
}
