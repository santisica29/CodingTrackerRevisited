using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTrackerRevisited;
internal class DatabaseManager
{
    internal void CreateTable(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var sql =
            @"CREATE TABLE IF NOT EXISTS coding (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    StartTime TEXT,
                    EndTime TEXT,
                    Duration TEXT
                )";

        connection.Execute(sql);
    }
}