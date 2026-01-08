namespace CodingTrackerRevisited;
internal class TableVisualisation
{
    internal static void ShowTable<T>(List<T> tableData) where T : class
    {
        foreach (var cr in tableData)
        {
            Console.WriteLine($"Id:{cr.Id} - Date: {cr.Date} - Duration: {cr.Duration}");
        }
    }

}