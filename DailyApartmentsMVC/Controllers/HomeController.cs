using DailyApartmentsMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Web;
using System.Xml.Linq;

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

        public IActionResult Index(string country, 
                                    string city, 
                                    int? minPrice,
                                    int? maxPrice,
                                    string? daterange,
                                    int? roomNumber, 
                                    int? sleepingPlacesNumber,
                                    int page = 1)
        {
            var properties = _context.Properties.Include(a => a.PropertyOwner);

            // daterange will contain the selected date range in the format "DD/MM/YYYY - DD/MM/YYYY"
            DateTime? startDate, endDate;
            if (!string.IsNullOrEmpty(daterange))
            {
                string[] dateRangeParts = daterange.Split(" - ");
                startDate = DateTime.Parse(dateRangeParts[0]);
                endDate = DateTime.Parse(dateRangeParts[1]);
            }

            // Build the LINQ query to filter the properties.
            var filteredProperties = properties.Where(p =>
                (string.IsNullOrEmpty(country) || p.Country == country) &&
                (string.IsNullOrEmpty(city) || p.City == city) &&
                (!minPrice.HasValue || p.Price >= minPrice) &&
                (!maxPrice.HasValue || p.Price <= maxPrice) &&
                (!roomNumber.HasValue || p.RoomNumber >= roomNumber) &&
                (!sleepingPlacesNumber.HasValue || p.SleepingPlaceNumber >= sleepingPlacesNumber));
            //(!startDate.HasValue || p. >= startDate) &&
            //(!endDate.HasValue || p.EndDate <= endDate));


            // Implement pagination
            int pageSize = 5;
            var model = filteredProperties.Skip((page - 1) * pageSize).Take(pageSize);
            

            // Retrieve form values from query string
            ViewBag.Country = country;
            ViewBag.City = city;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.DateRange = daterange;
            ViewBag.RoomNumber = roomNumber;
            ViewBag.SleepingPlacesNumber = sleepingPlacesNumber;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)filteredProperties.Count() / pageSize);
            ViewBag.OptionsFound = filteredProperties.Count();

            return View(model);
        }

        #region AutoCompleteActions
        public ActionResult CountryAutoComplete(string search)
        {
            var data = _context.Properties.GroupBy(p => p.Country).Select(grp => grp.First().Country);
            var result = data.Where(x => x.ToLower().StartsWith(search.ToLower()));
            return Json(result);
        }
        public ActionResult CityAutoComplete(string search)
        {
            var cities = _context.Properties.GroupBy(p => p.City).Select(grp => grp.First().City).ToList();
            var filteredCities = cities.Where(c => c.ToLower().StartsWith(search.ToLower()));
            return Json(filteredCities);
        }
        #endregion


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}