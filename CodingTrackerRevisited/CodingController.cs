
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

            tableCmd.Parameters.AddWithValue("@Date", newCodingRecord.Date);
            tableCmd.Parameters.AddWithValue("@Duration", newCodingRecord.Duration);

            tableCmd.ExecuteNonQuery();
            Console.WriteLine("Coding record added successfully!");
        }

        internal void Get()
        {
            List<CodingRecord> tableData = new List<CodingRecord>();

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
            {
                Console.WriteLine("No rows found");
            }
            else
            {
                TableVisualisation.ShowTable(tableData);
                foreach (var cr in tableData)
                {
                    Console.WriteLine($"Id:{cr.Id} - Date: {cr.Date} - Duration: {cr.Duration}");
                }
            }





        }
    }
}