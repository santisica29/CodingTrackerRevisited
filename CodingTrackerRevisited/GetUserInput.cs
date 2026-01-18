
using System.Globalization;

namespace CodingTrackerRevisited;
internal class GetUserInput
{
    CodingController codingController = new();
    internal void MainMenu()
    {
        bool closeApp = false;

        while (!closeApp)
        {
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("\nType 0 to close the app");
            Console.WriteLine("Type 1 to view records");
            Console.WriteLine("Type 2 to add records");
            Console.WriteLine("Type 3 to delete records");
            Console.WriteLine("Type 4 to update records\n");

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
                case "1":
                    codingController.Get();
                    break;
                case "2":
                    ProcessAdd();
                    break;
                case "3":
                    ProcessDelete();
                    break;
                case "4":
                    ProcessUpdate();
                    break;
                default:
                    Console.WriteLine("Invalid command. Type a num from 0 to 4.");
                    break;
            }
        }
    }

    private void ProcessUpdate()
    {
        codingController.Get();

        Console.WriteLine("Please type the id of the record you want to update (or 0 to return to Main Menu)");
        string commandInput = Console.ReadLine();

        while (!Int32.TryParse(commandInput,NumberStyles.None, CultureInfo.InvariantCulture, out _))
        {
            Console.WriteLine("Invalid input. Try again.");
            commandInput = Console.ReadLine();
        }

        int id = Int32.Parse(commandInput);

        if (id == 0) MainMenu();

        var coding = codingController.GetById(id);

        if (coding == null)
        {
            Console.WriteLine($"There's no record with Id: {id}. Try again.");
            ProcessUpdate();
            return;
        }

        string newDate = GetDateInput();
        string newDuration = GetDurationInput();

        coding.Date = newDate;
        coding.Duration = newDuration;

        int rowsAffected = codingController.Update(coding);

        if (rowsAffected > 0)
            Console.WriteLine($"Record with id {id} was successfully updated.");
        else
            Console.WriteLine($"No record found with id {id}.");
    }

    private void ProcessDelete()
    {
        codingController.Get();

        Console.WriteLine("Please type the id of the record you want to delete (or 0 to return to Main Menu)");
        string commandInput = Console.ReadLine();

        // NumberStyles.None no permite empty, white leading o numeros negativos
        while (!Int32.TryParse(commandInput, NumberStyles.None, CultureInfo.InvariantCulture, out _))
        {
            Console.WriteLine("Invalid input. Try again.");
            commandInput = Console.ReadLine();
        }

        int id = Int32.Parse(commandInput);

        if (id == 0) MainMenu();

        var coding = codingController.GetById(id);

        if (coding == null)
        {
            Console.WriteLine($"There's no record with Id: {id}. Try again.");
            ProcessDelete();
            return;
        }

        int rowsAffected = codingController.Delete(id);

        if (rowsAffected > 0)
            Console.WriteLine($"Record with id {id} was successfully deleted.");
        else
            Console.WriteLine($"No record found with id {id}.");

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

        if (dateInput == "0") MainMenu();

        while (!DateTime.TryParseExact(dateInput,"dd-MM-yy",CultureInfo.InvariantCulture,DateTimeStyles.None, out _)) 
        {
            Console.WriteLine("Invalid date. Try again, format (dd-mm-yy)");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }
}