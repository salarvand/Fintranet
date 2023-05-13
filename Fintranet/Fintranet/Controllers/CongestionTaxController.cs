using Fintranet.Models;
using Fintranet.Models.CongestionTaxCalculator;
using Microsoft.AspNetCore.Mvc;

namespace Fintranet.Controllers
{
    [ApiController]
    [Route("api/congestiontax")]
    public class CongestionTaxController : ControllerBase
    {
        private readonly CongestionTaxCalculator congestionTaxCalculator;

        public CongestionTaxController()
        {
            congestionTaxCalculator = new CongestionTaxCalculator();
        }

        // ####### Sample Request ##########
        //
        // request = new CongestionTaxRequest
        // {
        //     Vehicle = "Car",
        //     Dates = new DateTime[]
        //     {
        //         new DateTime(2013, 1, 14, 21, 0, 0),
        //         new DateTime(2013, 1, 15, 21, 0, 0),
        //         new DateTime(2013, 2, 7, 6, 23, 27),
        //         // Add more dates as needed
        //     },
        //     City = "Gothenburg"
        // };

        [HttpPost]
        public ActionResult<int> Calculate([FromBody] CongestionTaxRequest request)
        {
            try
            {
                int tax = congestionTaxCalculator.CalculateCongestionTax(request.City, new Vehicle(request.Vehicle), request.Dates);
                return Ok(tax);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while calculating congestion tax.");
            }
        }
    }
}
