
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

            using var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = "INSERT INTO coding (date, duration) VALUES (@Date, @Duration)";

            tableCmd.Parameters.Add("@Date", SqliteType.Text).Value = newCodingRecord.Date;
            tableCmd.Parameters.Add("@Duration", SqliteType.Text).Value = newCodingRecord.Duration;

            int rowsAffected = tableCmd.ExecuteNonQuery();

            if (rowsAffected > 0)
                Console.WriteLine("Coding record added successfully!");
            else
                Console.WriteLine("Record couldn't be added.");
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

        internal CodingRecord? GetById(int id)
        {
            using var connection = new SqliteConnection(connectionString);

            connection.Open();
            using var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = "SELECT * FROM coding WHERE Id = @Id";

            tableCmd.Parameters.Add("@Id", SqliteType.Integer).Value = id;

            using var reader = tableCmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return new CodingRecord
            {
                Id = reader.GetInt32(0),
                Date = reader.GetString(1),
                Duration = reader.GetString(2)
            };
        }

        internal int Delete(int id)
        {
            using var connection = new SqliteConnection(connectionString);

            connection.Open();
            using var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "DELETE FROM coding WHERE Id = @Id";

            tableCmd.Parameters.Add("@Id", SqliteType.Integer).Value = id;

            return tableCmd.ExecuteNonQuery();
        }

        internal int Update(CodingRecord cr)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "UPDATE coding SET Date = @Date, Duration = @Duration WHERE Id = @Id";

            tableCmd.Parameters.Add("@Date", SqliteType.Text).Value = cr.Date;
            tableCmd.Parameters.Add("@Duration", SqliteType.Text).Value = cr.Duration;
            tableCmd.Parameters.Add("@Id", SqliteType.Integer).Value = cr.Id;

            return tableCmd.ExecuteNonQuery();
        }
    }
}