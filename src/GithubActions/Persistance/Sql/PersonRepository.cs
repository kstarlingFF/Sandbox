using Dapper;

namespace GithubActions.Persistance.Sql;

public interface IPersonRepository : IRepository<Person>
{
}

public class PersonRepository : IPersonRepository
{
    private readonly IDatabaseConnection _conn;

    public PersonRepository(IDatabaseConnection connection)
    {
        _conn = connection;
    }

    public async virtual Task<Person> FindAsync(int id)
    {
        var query = @"
            SELECT *
            FROM [Person] 
            WHERE Id = @id ";

        return await _conn.Connection.QueryFirstOrDefaultAsync<Person>(query, new { id });
    }
}
