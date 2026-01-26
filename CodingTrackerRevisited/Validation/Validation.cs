using Spectre.Console;
using System.Globalization;

namespace CodingTrackerRevisited.Validation;
internal static class Validation
{
    internal static string GetDuration(string startTime, string endTime)
    {
        var duration = TimeSpan.Parse(endTime).Subtract(TimeSpan.Parse(startTime));

        return duration.ToString("hh\\:mm");
    }

    internal static bool IsEndBeforeStart(string startTime, string endTime)
    {
        return TimeSpan.Parse(endTime) < TimeSpan.Parse(startTime);
    }

    internal static string GetTimeInput(string msg)
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

        return timeInput;
    }

    internal static string GetDateInput()
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

        if (dateInput == "t")
            return DateTime.Now.ToString("yyyy-MM-dd");

        //Parse the string to datetime using the "current format" not the one i want to show, for that i use toString afterwards
        return DateTime.ParseExact(dateInput, "dd-MM-yy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
    }
}
