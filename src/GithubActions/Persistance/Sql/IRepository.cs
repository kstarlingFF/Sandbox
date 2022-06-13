namespace GithubActions.Persistance.Sql;

public interface IRepository<TEntity>
{
    Task<TEntity> FindAsync(int Id);
}
