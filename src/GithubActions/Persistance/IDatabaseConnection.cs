using System.Data;

namespace GithubActions.Persistance;

public interface IDatabaseConnection
{
    IDbConnection Connection { get; }
}

public class DatabaseConnection : IDatabaseConnection
{
    public IDbConnection Connection { get; }

    public DatabaseConnection(IDbConnection connection)
    {
        Connection = connection;
    }
}
