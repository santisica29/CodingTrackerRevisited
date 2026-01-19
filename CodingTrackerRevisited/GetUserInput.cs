
using Spectre.Console;
using System.Globalization;
using static CodingTrackerRevisited.Enums.Enums;

namespace CodingTrackerRevisited;
internal class GetUserInput
{
    CodingController codingController = new();
    internal void MainMenu()
    {
        bool closeApp = false;

        while (!closeApp)
        {
            var commandInput = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOptions>()
                .Title("Select an option")
                .UseConverter<MenuOptions>(x => ToDescriptionString(x))
                .AddChoices(Enum.GetValues<MenuOptions>())
                );

            switch (commandInput)
            {
                case MenuOptions.Exit:
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case MenuOptions.ViewRecords:
                    codingController.Get();
                    break;
                case MenuOptions.InsertRecords:
                    ProcessAdd();
                    break;
                case MenuOptions.DeleteRecords:
                    ProcessDelete();
                    break;
                case MenuOptions.UpdateRecords:
                    ProcessUpdate();
                    break;
            }
        }
    }

    private void ProcessUpdate()
    {
        codingController.Get();

        string commandInput = AnsiConsole.Prompt(
            new TextPrompt<string>("Please type the id of the record you want to update (or 0 to return to Main Menu)")
            .Validate(input =>
            {
                if (!Int32.TryParse(input, NumberStyles.None, CultureInfo.InvariantCulture, out _))
                    return ValidationResult.Error("Invalid input");
                else
                    return ValidationResult.Success();
            }));

        int id = Int32.Parse(commandInput);

        if (id == 0) MainMenu();

        var coding = codingController.GetById(id);

        if (coding == null)
        {
            AnsiConsole.MarkupLine($"There's no record with Id: {id}. Try again.");
            ProcessUpdate();
            return;
        }

        bool updating = true;

        while (updating)
        {
            var cmd = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Choose which property you want to update.")
                .AddChoices("Date", "Duration","Stop updating", "Back to Main Menu")
                );

            switch (cmd)
            {
                case "Date":
                    coding.Date = GetDateInput();
                    break;
                case "Duration":
                    coding.Duration = GetDurationInput();
                    break;
                case "Stop updating":
                    updating = false;
                    break;
                case "Back to Main Menu":
                    updating = false;
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

        int rowsAffected = codingController.Update(coding);

        if (rowsAffected > 0)
            AnsiConsole.MarkupLine($"Record with id {id} was successfully updated.");
        else
            AnsiConsole.MarkupLine($"[red]No record found with id {id}.[/]");
    }

    private void ProcessDelete()
    {
        codingController.Get();

        string commandInput = AnsiConsole.Prompt(
            new TextPrompt<string>("Please type the id of the record you want to delete (or 0 to return to Main Menu)")
            .Validate(input =>
            {
                if (!Int32.TryParse(input, NumberStyles.None, CultureInfo.InvariantCulture, out _))
                    return ValidationResult.Error("Invalid input");
                else
                    return ValidationResult.Success();
            }));

        int id = Int32.Parse(commandInput);

        if (id == 0) MainMenu();

        var coding = codingController.GetById(id);

        if (coding == null)
        {
            AnsiConsole.MarkupLine($"[red]There's no record with Id: {id}. Try again.[/]");
            ProcessDelete();
            return;
        }

        int rowsAffected = codingController.Delete(id);

        if (rowsAffected > 0)
            AnsiConsole.MarkupLine($"[#5FFFD7]Record with id {id} was successfully deleted.[/]");
        else
            AnsiConsole.MarkupLine($"[red]No record found with id {id}.[/]");
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
        var durationInput = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the duration. Format [green](hh:mm)[/]. Type 0 to go back to main menu")
                .Validate(input =>
                {
                    if (!TimeSpan.TryParseExact(input, "h\\:mm", CultureInfo.InvariantCulture, out _))
                        return ValidationResult.Error("Invalid date. Try again, format (dd-mm-yy)");
                    else
                        return ValidationResult.Success();
                }));

        if (durationInput == "0") MainMenu();

        return durationInput;
    }

    private string GetDateInput()
    {
        var dateInput = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter a date. Format [green](dd-mm-yy)[/]. Type 0 to go back to main menu")
                .Validate(input =>
                {
                    if (!DateTime.TryParseExact(input, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                        return ValidationResult.Error("Invalid date. Try again, format (dd-mm-yy)");
                    else
                        return ValidationResult.Success();
                }));

        if (dateInput == "0") MainMenu();

        return dateInput;
    }
}