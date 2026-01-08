
using System.Globalization;

namespace CodingTrackerRevisited;
internal class GetUserInput
{
    CodingController codingController = new();
    internal void MainMenu()
    {
        bool closeApp = false;

        while (closeApp == false)
        {
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("\nType 0 to close the app");
            Console.WriteLine("Type 1 to view records");
            Console.WriteLine("Type 2 to add records");
            Console.WriteLine("Type 3 to delete records");
            Console.WriteLine("Type 4 to update records");

            string commandInput = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(commandInput))
            {
                Console.WriteLine("Invalid option.");
                commandInput = Console.ReadLine();
            } 

            switch (commandInput)
            {
                case "0":
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                //case "1":
                //    codingController.Get();
                //    break;
                case "2":
                    ProcessAdd();
                    break;
                //case "3":
                //    ProcessDelete();
                //    break;
                //case "4":
                //    ProcessUpdate();
                //    break;
                default:
                    Console.WriteLine("Invalid command. Type a num from 0 to 4.");
                    break;
            }
                

        }

    }

    private void ProcessAdd()
    {
        string date = GetDateInput();
        string duration = GetDurationInput();

        CodingRecord newCodingRecord = new();

        newCodingRecord.Date = date;
        newCodingRecord.Duration = duration;

        codingController.Post(newCodingRecord);
    }

    private string GetDurationInput()
    {
        Console.WriteLine("Insert the duration. Format (hh:mm). Type 0 to return to main menu");
        string durationInput = Console.ReadLine();

        if (durationInput == "0") MainMenu();

        while(!TimeSpan.TryParseExact(durationInput, "h\\:mm", CultureInfo.InvariantCulture, out _))
        {
            Console.WriteLine("Invalid format. Try again (hh:mm).");
            durationInput = Console.ReadLine();
        }

        return durationInput;
    }

    private string GetDateInput()
    {
        Console.WriteLine("Enter a date. Format (dd-mm-yy). Type 0 to go back to main menu");
        string dateInput = Console.ReadLine();

        if (dateInput == "0")
        {
            MainMenu();
        }

        while (!DateTime.TryParseExact(dateInput,"dd-MM-yy",CultureInfo.InvariantCulture,DateTimeStyles.None, out _)) 
        {
            Console.WriteLine("Invalid date. Try again, format (dd-mm-yy)");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }
}