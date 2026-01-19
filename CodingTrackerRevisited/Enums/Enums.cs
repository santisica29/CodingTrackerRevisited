using System;
using System.ComponentModel;

namespace CodingTrackerRevisited.Enums;

public static class Enums
{
    public enum MenuOptions
    {
        [Description("View Records")]
        ViewRecords,
        [Description("Insert Records")]
        InsertRecords,
        [Description("Delete Records")]
        DeleteRecords,
        [Description("Update Records")]
        UpdateRecords,
        [Description("Exit")]
        Exit
    }

    public static string ToDescriptionString(Enum en)
    {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])en
           .GetType()
           .GetField(en.ToString())
           .GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }
}
