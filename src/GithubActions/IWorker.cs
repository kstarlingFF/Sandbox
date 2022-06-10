namespace GithubActions;

public interface IWorker
{
    Task<string> GetNameAsync(int id);
}
