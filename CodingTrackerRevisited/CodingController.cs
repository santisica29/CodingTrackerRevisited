
using Microsoft.Data.Sqlite;
using Dapper;
using System.Configuration;

namespace CodingTrackerRevisited
{
    internal class CodingController
    {
        string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        internal int Post(CodingRecord newCodingRecord)
        {
            using var connection = new SqliteConnection(connectionString);
            
            var sql = "INSERT INTO coding (date, duration) VALUES (@Date, @Duration)";

            return connection.Execute(sql, new {Date = newCodingRecord.Date, Duration = newCodingRecord.Duration});   
        }

        internal List<CodingRecord> Get()
        {
            using var connection = new SqliteConnection(connectionString);

            var sql = "SELECT * FROM coding";

            var list = connection.Query<CodingRecord>(sql).ToList();

            return list;
        }

        internal CodingRecord? GetById(int id)
        {
            using var connection = new SqliteConnection(connectionString);

            var sql = "SELECT * FROM coding WHERE Id = @Id";

            // OrDefault when null is a possibility
            var record = connection.QuerySingleOrDefault<CodingRecord>(sql, new { Id = id });

            return record;
        }

        internal int Delete(int id)
        {
            using var connection = new SqliteConnection(connectionString);

            var sql = "DELETE FROM coding WHERE Id = @Id";

            return connection.Execute(sql, new { Id = id});
        }

        internal int Update(CodingRecord cr)
        {
            using var connection = new SqliteConnection(connectionString);

            var sql = "UPDATE coding SET Date = @Date, Duration = @Duration WHERE Id = @Id";

            return connection.Execute(sql, new { Date = cr.Date, Duration = cr.Duration, Id = cr.Id });
        }
    }
}