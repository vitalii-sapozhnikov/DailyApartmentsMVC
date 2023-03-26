using DailyApartmentsMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Web;

namespace DailyApartmentsMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AirbnbContext _context;

        public HomeController(ILogger<HomeController> logger, AirbnbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string country, string city, int? price, DateTime? startDate, DateTime? endDate, int? roomNumber, int? sleepingPlacesNumber)
        {
            var properties = _context.Properties.Include(a => a.PropertyOwner);

            // Build the LINQ query to filter the properties.
            var filteredProperties = properties.Where(p =>
                (string.IsNullOrEmpty(country) || p.Country == country) &&
                (string.IsNullOrEmpty(city) || p.City == city) &&
                (!price.HasValue || p.Price <= price) &&
                (!roomNumber.HasValue || p.RoomNumber >= roomNumber) &&
                (!sleepingPlacesNumber.HasValue || p.SleepingPlaceNumber >= sleepingPlacesNumber));

            int numberOfSuggestions = filteredProperties.Count();
            ViewBag.Message = $"Було знайдено ${numberOfSuggestions} пропозицій по вашому запиту!";

            return View(filteredProperties);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}