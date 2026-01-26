using System.ComponentModel;

namespace CodingTrackerRevisited.Models;
internal class CodingRecord
{
    public int Id { get; set; }
    public string Date { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Duration { get; set; }
}

