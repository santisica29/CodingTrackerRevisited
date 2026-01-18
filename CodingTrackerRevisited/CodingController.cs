
using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTrackerRevisited
{
    internal class CodingController
    {
        string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        internal void Post(CodingRecord newCodingRecord)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = "INSERT INTO coding (date, duration) VALUES (@Date, @Duration)";

            tableCmd.Parameters.Add("@Date", SqliteType.Text).Value = newCodingRecord.Date;
            tableCmd.Parameters.Add("@Duration", SqliteType.Text).Value = newCodingRecord.Duration;

            tableCmd.ExecuteNonQuery();
            Console.WriteLine("Coding record added successfully!");
        }

        internal void Get()
        {
            List<CodingRecord> tableData = new();

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT * FROM coding";

            using var reader = tableCmd.ExecuteReader();

            while (reader.Read())
            {
                tableData.Add(new CodingRecord
                {
                    Id = reader.GetInt32(0),
                    Date = reader.GetString(1),
                    Duration = reader.GetString(2)
                });
            }

            if (tableData.Count == 0)
                Console.WriteLine("No rows found");
            else
                TableVisualisation.ShowTable(tableData);
        }

        internal CodingRecord GetById(int id)
        {
            using var connection = new SqliteConnection(connectionString);

            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = "SELECT * FROM coding WHERE Id = @Id";

            tableCmd.Parameters.Add("@Id", SqliteType.Integer).Value = id;

            var reader = tableCmd.ExecuteReader();

            CodingRecord record = new();

            if (reader.Read())
            {
                record.Id = reader.GetInt32(0);
                record.Date = reader.GetString(1);
                record.Duration = reader.GetString(2);
            }

            Console.WriteLine("\n\n");

            return record;
        }

        internal void Delete(int id)
        {
            using var connection = new SqliteConnection(connectionString);

            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "DELETE FROM coding WHERE Id = @Id";

            tableCmd.Parameters.Add("@Id", SqliteType.Integer).Value = id;

            int rowsAffected = tableCmd.ExecuteNonQuery();

            if (rowsAffected > 0) 
                Console.WriteLine($"Record with id {id} was successfully deleted.");
            else
                Console.WriteLine($"No record found with id {id}.");
        }
    }
}