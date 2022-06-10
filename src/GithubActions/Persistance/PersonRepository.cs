using Dapper;

namespace GithubActions.Persistance;

public interface IPersonRepository : IRepository<Person>
{
}

public class PersonRepository : IPersonRepository
{
    private readonly IDatabaseConnection _conn;

    public PersonRepository(IDatabaseConnection connection)
    {
        this._conn = connection;
    }

    public async virtual Task<Person> FindAsync(int id)
    {
        var query = @"
            SELECT *
            FROM [Person] 
            WHERE Id = @id ";

        return await this._conn.Connection.QueryFirstOrDefaultAsync<Person>(query, new { id = id });
    }
}
