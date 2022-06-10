namespace GithubActions.Persistance;

public interface IRepository<TEntity>
{
    Task<TEntity> FindAsync(int Id);
}
