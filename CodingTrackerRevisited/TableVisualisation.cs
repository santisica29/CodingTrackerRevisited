using ConsoleTableExt;

namespace CodingTrackerRevisited;
internal class TableVisualisation
{
    internal static void ShowTable<T>(List<T> tableData) where T : class
    {
        Console.WriteLine("\n\n");

        ConsoleTableBuilder
            .From(tableData)
            .WithTitle("Coding Records", ConsoleColor.DarkRed)
            .ExportAndWriteLine();

        Console.WriteLine("\n\n");
    }

}