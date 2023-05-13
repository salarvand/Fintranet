using Fintranet.Models.CongestionTaxCalculator;
using Newtonsoft.Json;

namespace Fintranet.Models;

public class CityTaxRules
{
    [JsonProperty("TaxIntervals")]
    public List<TimeInterval> TaxIntervals { get; set; }

    [JsonProperty("ExemptDates")]
    public List<string> ExemptDates { get; set; }

    [JsonProperty("ExemptDaysOfWeek")]
    public List<int> ExemptDaysOfWeek { get; set; }

    [JsonProperty("MaxFeePerDay")]
    public int MaxFeePerDay { get; set; }

    public bool IsExemptDate(DateTime date)
    {
        string monthDay = date.ToString("MM-dd");
        return ExemptDates.Contains(monthDay) || ExemptDaysOfWeek.Contains((int)date.DayOfWeek);
    }

    public int GetTollFee(DateTime date)
    {
        foreach (var interval in TaxIntervals)
        {
            if (interval.IsWithinInterval(date))
            {
                return interval.Amount;
            }
        }

        return 0;
    }
}