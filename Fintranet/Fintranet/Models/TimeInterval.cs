using Newtonsoft.Json;

namespace Fintranet.Models.CongestionTaxCalculator;

public class TimeInterval
{
    [JsonProperty("StartTime")]
    public string StartTime { get; set; }

    [JsonProperty("EndTime")]
    public string EndTime { get; set; }

    [JsonProperty("Amount")]
    public int Amount { get; set; }

    public TimeInterval(string startTime, string endTime, int amount)
    {
        StartTime = startTime;
        EndTime = endTime;
        Amount = amount;
    }

    public bool IsWithinInterval(DateTime dateTime)
    {
        TimeSpan startTime = TimeSpan.Parse(StartTime);
        TimeSpan endTime = TimeSpan.Parse(EndTime);
        TimeSpan currentTime = dateTime.TimeOfDay;

        // Handle the case when the interval spans across two different days
        if (endTime < startTime)
        {
            return currentTime >= startTime || currentTime <= endTime;
        }

        return currentTime >= startTime && currentTime <= endTime;
    }
}