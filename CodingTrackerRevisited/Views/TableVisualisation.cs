using ConsoleTableExt;

namespace CodingTrackerRevisited.Views;
internal class TableVisualisation
{
    internal static void ShowTable<T>(List<T> tableData, string title = "Coding Records") where T : class
    {
        Console.WriteLine("\n\n");

        ConsoleTableBuilder
            .From(tableData)
            .WithTitle(title, ConsoleColor.DarkGreen)
            .ExportAndWriteLine();

        Console.WriteLine("\n\n");
    }

}