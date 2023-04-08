using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DailyApartmentsMVC.Models;
using DailyApartmentsMVC.Models.GuestModel;
using System.Diagnostics;
using DailyApartmentsMVC.AppSettings;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace DailyApartmentsMVC.Controllers
{
    [Authorize]
    public class GuestController : Controller
    {
        private readonly IServiceProvider _serviceProvider;

        public GuestController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IActionResult Search(string country,
                                    string city,
                                    int? minPrice,
                                    int? maxPrice,
                                    string? daterange,
                                    int? roomNumber,
                                    int? sleepingPlacesNumber,
                                    int page = 1)
        {
            var properties = AppSettings.AppSettings.guestContext.PropertyLists;

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

        [HttpGet]
        public IActionResult Details(int id)
        {
            //var lst = AppSettings.AppSettings.guestContext.GetPropertyDetails(id);
            var propertyDetails = AppSettings.AppSettings.guestContext.PropertyDetails.FromSqlRaw("SELECT * FROM get_property_details({0})", id).ToList();



            return View(propertyDetails);
        }

        [HttpGet]
        public IActionResult Bookings() 
        {
            var bookings_list = AppSettings.AppSettings.guestContext.BookingsArchives.ToList();

            return View(bookings_list);
        }

        #region AutoCompleteActions
        public ActionResult CountryAutoComplete(string search)
        {
            var data = AppSettings.AppSettings.guestContext.PropertyLists.GroupBy(p => p.Country).Select(grp => grp.First().Country);
            var result = data.Where(x => x.ToLower().StartsWith(search.ToLower()));
            return Json(result);
        }
        public ActionResult CityAutoComplete(string search)
        {
            var cities = AppSettings.AppSettings.guestContext.PropertyLists.GroupBy(p => p.City).Select(grp => grp.First().City).ToList();
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
