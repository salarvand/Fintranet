namespace Fintranet.Models;

public class CongestionTaxRequest
{
    public string City { get; set; }
    public DateTime[] Dates { get; set; }
    public string Vehicle { get; set; }
}