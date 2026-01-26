using Microsoft.Data.Sqlite;
using Dapper;
using System.Configuration;
using CodingTrackerRevisited.Models;

namespace CodingTrackerRevisited.Controllers;
internal class CodingController
{
    string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

    internal int Post(CodingRecord newCodingRecord)
    {
        using var connection = new SqliteConnection(connectionString);

        var sql = "INSERT INTO coding (Date, StartTime, EndTime, Duration) VALUES (@Date, @StartTime, @EndTime, @Duration)";

        return connection.Execute(sql, new
        {
            Date = newCodingRecord.Date,
            StartTime = newCodingRecord.StartTime,
            EndTime = newCodingRecord.EndTime,
            Duration = newCodingRecord.Duration
        });
    }

    internal List<CodingRecord> Get()
    {
        using var connection = new SqliteConnection(connectionString);

        var sql = "SELECT * FROM coding";

        return connection.Query<CodingRecord>(sql).ToList();
    }

    internal List<CodingRecord> GetFilteredList(string timeCondition, int durationCondition)
    {
        using var connection = new SqliteConnection(connectionString);

        var sql = $"SELECT * FROM coding WHERE Date > date('now', '-{durationCondition.ToString()} {timeCondition}')";

        return connection.Query<CodingRecord>(sql).ToList();
    }

    internal CodingRecord? GetById(int id)
    {
        using var connection = new SqliteConnection(connectionString);

        var sql = "SELECT * FROM coding WHERE Id = @Id";

        // OrDefault when null is a possibility
        return connection.QuerySingleOrDefault<CodingRecord>(sql, new { Id = id });
    }

    internal int Delete(int id)
    {
        using var connection = new SqliteConnection(connectionString);

        var sql = "DELETE FROM coding WHERE Id = @Id";

        return connection.Execute(sql, new { Id = id });
    }

    internal int Update(CodingRecord cr)
    {
        using var connection = new SqliteConnection(connectionString);

        var sql = @"UPDATE coding
                SET Date = @Date, StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration 
                WHERE Id = @Id";

        return connection.Execute(sql, new
        {
            Date = cr.Date,
            StartTime = cr.StartTime,
            EndTime = cr.EndTime,
            Duration = cr.Duration,
            Id = cr.Id
        });
    }
}