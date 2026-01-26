using Dapper;
using Microsoft.Data.Sqlite;
using System.Text;

namespace CodingTrackerRevisited.Services;
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

//select* from coding where Date > date('now', '-1 month')