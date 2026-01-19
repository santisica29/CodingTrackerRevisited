
using Spectre.Console;
using System.Diagnostics;
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
                .Title("[blue]MAIN MENU[/]")
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
                    ProcessGet();
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
                case MenuOptions.StartRecord:
                    ProcessStartRecord();
                    break;
            }
        }
    }

    private void ProcessStartRecord()
    {
        AnsiConsole.MarkupLine("Press any key to start the timer");
        Console.ReadKey();

        var sw = Stopwatch.StartNew();

        var startTime = DateTime.Now.ToString("hh:mm");
        string endTime = String.Empty;

        bool timeIsRunning = true;

        while (timeIsRunning)
        {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices("Pause", "Resume", "Stop and save", "Restart", "Go back")
            );

            switch (choice)
            {
                case "Pause":
                    sw.Stop();
                    break;
                case "Resume":
                    sw.Start();
                    break;
                case "Stop and save":
                    sw.Stop();
                    endTime = DateTime.Now.ToString("hh:mm");
                    timeIsRunning = false;
                    break;
                case "Restart":
                    sw.Restart();
                    break;
                case "Go back":
                    return;
            }
        }

        var time = sw.Elapsed;

        string elapsedTime = time.ToString("hh\\:mm\\:ss");

        AnsiConsole.MarkupLine($"Your session: {elapsedTime}");

        var command = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Are you sure you want to save this session?")
            .AddChoices("yes", "no")
            );

        if (command == "no")
        {
            AnsiConsole.MarkupLine("Your session won't be saved. Press any key to go back to the main menu");
            Console.ReadLine();
            return;
        }

        CodingRecord cr = new CodingRecord
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            StartTime = startTime,
            EndTime = endTime,
            Duration = GetDuration(startTime, endTime),
        };

        int rowsAffected = codingController.Post(cr);

        if (rowsAffected > 0)
            AnsiConsole.MarkupLine("[green]Coding record added successfully![/]");
        else
            AnsiConsole.MarkupLine("Record couldn't be added.");
    }

    private void ProcessUpdate()
    {
        AnsiConsole.Clear();

        ProcessGet();

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
            AnsiConsole.MarkupLine($"[red]There's no record with Id: {id}. Try again.[/]");
            Console.ReadLine();
            ProcessUpdate();
            return;
        }

        bool updating = true;

        while (updating)
        {
            var cmd = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Choose which property you want to update.")
                .AddChoices("Date", "Start Time", "End Time", "Stop updating", "Back to Main Menu")
                );

            switch (cmd)
            {
                case "Date":
                    coding.Date = GetDateInput();
                    break;
                case "Start Time":
                    coding.StartTime = GetTimeInput("start time");
                    break;
                case "End Time":
                    coding.EndTime = GetTimeInput("end time");
                    while (IsEndBeforeStart(coding.StartTime, coding.EndTime))
                    {
                        AnsiConsole.MarkupLine($"[red]Invalid input. Please make sure the end time is later than the start time{coding.StartTime}\r\n\r\n[/]");
                        coding.EndTime = GetTimeInput("end time");
                    }
                    break;
                case "Stop updating":
                    coding.Duration = GetDuration(coding.StartTime, coding.EndTime);
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
        AnsiConsole.Clear();

        ProcessGet();

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
        AnsiConsole.Clear();

        string date = GetDateInput();
        string startTime = GetTimeInput("start time");
        string endTime = GetTimeInput("end time");

        while (IsEndBeforeStart(startTime, endTime))
        {
            AnsiConsole.MarkupLine("End time must be higher than start time");
            endTime = GetTimeInput("end time");
        }

        string duration = GetDuration(startTime, endTime);

        CodingRecord newCodingRecord = new()
        {
            Date = date,
            StartTime = startTime,
            EndTime = endTime,
            Duration = duration
        };

        int rowsAffected = codingController.Post(newCodingRecord);

        if (rowsAffected > 0)
            AnsiConsole.MarkupLine("[green]Coding record added successfully![/]");
        else
            AnsiConsole.MarkupLine("Record couldn't be added.");
    }

    private string GetDuration(string startTime, string endTime)
    {
        var duration = TimeSpan.Parse(endTime).Subtract(TimeSpan.Parse(startTime));
        
        return duration.ToString("hh\\:mm");
    }

    private bool IsEndBeforeStart(string startTime, string endTime)
    {
        return TimeSpan.Parse(endTime) < TimeSpan.Parse(startTime);
    }

    private void ProcessGet()
    {
        AnsiConsole.Clear();

        var list = codingController.Get();

        if (list.Count == 0)
            AnsiConsole.MarkupLine("[DarkRed]No rows found[/]");
        else
            TableVisualisation.ShowTable(list);
    }

    private string GetTimeInput(string msg)
    {
        var timeInput = AnsiConsole.Prompt(
            new TextPrompt<string>($"Enter the {msg}. Format [green](hh:mm)[/]. Type 0 to go back to main menu")
                .Validate(input =>
                {
                    if (!TimeSpan.TryParseExact(input, "hh\\:mm", CultureInfo.CurrentCulture, out _))
                        return ValidationResult.Error("Invalid date. Try again, format (hh:mm)");
                    else
                        return ValidationResult.Success();
                }));

        if (timeInput == "0") MainMenu();

        return timeInput;
    }

    private string GetDateInput()
    {
        var dateInput = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter a date. Format [green](dd-MM-yy)[/]. Type 't' to enter today's date. Type 0 to go back to main menu")
                .Validate(input =>
                {
                    if (input.Trim().Equals("t"))
                        return ValidationResult.Success();

                    if (!DateTime.TryParseExact(input, "dd-MM-yy", CultureInfo.CurrentCulture, DateTimeStyles.None, out _))
                        return ValidationResult.Error("Invalid date. Try again, format (dd-MM-yy)");
                    
                    return ValidationResult.Success();
                }));

        if (dateInput == "0") MainMenu();

        if (dateInput == "t")
            dateInput = DateTime.Now.ToString();

        var finalDate = DateTime.Parse(dateInput).ToString("yyyy-MM-dd");

        return finalDate;
    }
}