using Codecool.BruteForce.Users.Model;
using Microsoft.Data.Sqlite;

namespace Codecool.BruteForce.Users.Repository;

public class UserRepository : IUserRepository
{
    private readonly string _dbFilePath;

    public UserRepository(string dbFilePath)
    {
        _dbFilePath = dbFilePath;
    }

    private SqliteConnection GetPhysicalDbConnection()
    {
        var dbConnection = new SqliteConnection($"Data Source ={_dbFilePath};Mode=ReadWrite");
        dbConnection.Open();
        return dbConnection;
    }

    private void ExecuteNonQuery(string query)
    {
        using var connection = GetPhysicalDbConnection();
        using var command = GetCommand(query, connection);
        command.ExecuteNonQuery();
    }

    private static SqliteCommand GetCommand(string query, SqliteConnection connection)
    {
        return new SqliteCommand
        {
            CommandText = query,
            Connection = connection,
        };
    }

    public void Add(string userName, string password)
    {
        var query = @$"INSERT INTO users (user_name, password) VALUES ('{userName}', '{password}')";
        ExecuteNonQuery(query);
    }

    public void Update(int id, string userName, string password)
    {
        var query = @$"UPDATE users SET user_name = '{userName}', password = '{password}' WHERE Id = {id}";
        ExecuteNonQuery(query);
    }

    public void Delete(int id)
    {
        var query = @$"DELETE FROM users WHERE Id = {id}";
        ExecuteNonQuery(query);
    }

    public void DeleteAll()
    {
        var query = @"DELETE FROM users";
        ExecuteNonQuery(query);
    }

    public User Get(int id)
    {
        var query = @$"SELECT * FROM users WHERE id = {id}";
        using var connection = GetPhysicalDbConnection();
        using var command = GetCommand(query, connection);

        using var reader = command.ExecuteReader();
        return new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
    }

    public IEnumerable<User> GetAll()
    {
        var users = new List<User>();
        var query = @"SELECT * FROM users";
    
        using var connection = GetPhysicalDbConnection();
        using var command = GetCommand(query, connection);
    
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            users.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
        }
    
        return users;
    }
}
