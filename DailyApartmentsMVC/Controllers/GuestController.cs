using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DailyApartmentsMVC.Models;
using DailyApartmentsMVC.Models.GuestModel;
using System.Diagnostics;
using DailyApartmentsMVC.AppSettings;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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
            DateOnly startDate = DateOnly.MinValue, endDate = DateOnly.MaxValue;
            if (!string.IsNullOrEmpty(daterange))
            {
                string[] dateRangeParts = daterange.Split(" - ");
                startDate = DateOnly.Parse(dateRangeParts[0]);
                endDate = DateOnly.Parse(dateRangeParts[1]);
            }

            // Build the LINQ query to filter the properties.
            var filteredProperties = properties.Where(p =>
                (string.IsNullOrEmpty(country) || p.Country == country) &&
                (string.IsNullOrEmpty(city) || p.City == city) &&
                (!minPrice.HasValue || p.Price >= minPrice) &&
                (!maxPrice.HasValue || p.Price <= maxPrice) &&
                (!roomNumber.HasValue || p.RoomNumber >= roomNumber) &&
                (!sleepingPlacesNumber.HasValue || p.SleepingPlaceNumber >= sleepingPlacesNumber)).ToList();

            var notOverlappingProperties = filteredProperties.ToList();

            if (startDate != DateOnly.MinValue && endDate != DateOnly.MaxValue)
            {
                notOverlappingProperties = filteredProperties.Where(p =>
                (p.Dates?.Length ?? 0) == (p.Durations?.Length ?? 0) &&

                (!p.Dates?.Where((d, i) => (startDate < d.AddDays(p.Durations[i]) && endDate > d)
                || (startDate >= d && startDate < d.AddDays(p.Durations[i]))).Any() ?? true)
                ).ToList();

                filteredProperties = notOverlappingProperties.ToList();
            }


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

            ViewBag.Id = $"{id}";
            return View(propertyDetails);
        }

        [HttpGet]
        public IActionResult Bookings() 
        {
            var bookings_list = AppSettings.AppSettings.guestContext.BookingsArchives.ToList();

            List<string> review_attribute_names = AppSettings.AppSettings.guestContext.ReviewAttributes.Select(a => a.Name).ToList() ?? new List<string>();

            ViewBag.ReviewAttributes = review_attribute_names;


            List<PropertyComment> comments = AppSettings.AppSettings.guestContext.PropertyComments.ToList() ?? new List<PropertyComment>();
            List<PropertyReview> reviews = AppSettings.AppSettings.guestContext.PropertyReviews.ToList() ?? new List<PropertyReview>();


            ViewBag.Comments = comments;
            ViewBag.Reviews = reviews;

            return View(bookings_list);
        }

        [HttpPost]
        public IActionResult CreateBooking(int id, [FromForm(Name = "daterange")] string daterange)
        {

            DateTime startDate = DateTime.Now, endDate = DateTime.Now;
            if (!string.IsNullOrEmpty(daterange))
            {
                string[] dateRangeParts = daterange.Split(" - ");
                startDate = DateTime.Parse(dateRangeParts[0]);
                endDate = DateTime.Parse(dateRangeParts[1]);                
            }

            try
            {
                FormattableString sqlQuery = $"CALL create_booking({id}, {DateOnly.FromDateTime(startDate.Date)}, {DateOnly.FromDateTime(endDate.Date)})";
                AppSettings.AppSettings.guestContext.Database.ExecuteSql(sqlQuery);
            }
            catch(Exception ex)
            {
                TempData["DateRange"] = daterange;
                TempData["Error"] = "На жаль ці дати вже зайняті. Оберіть інші";
                return RedirectToAction("Details", new { id = id });
            }

            TempData["showToast"] = true;
            return RedirectToAction("Details", new { id = id});
        }


        public IActionResult Cancel(int id)
        {
            var result = AppSettings.AppSettings.guestContext.Database.ExecuteSqlInterpolated(
                $"SELECT cancel_booking({id})");

            return RedirectToAction("Bookings");
        }

        [HttpPost]
        public IActionResult SendReview(Dictionary<string, int> rates, string comment, int id)
        {
            if(rates != null)
            {
                foreach (var r in rates)
                {
                    int attr_id = int.Parse(r.Key.Substring(0, 1)) + 1;
                    var query = FormattableStringFactory.Create($@"INSERT INTO property_review (booking_id, review_attribute_id, value)" +
                        $"VALUES ({id}, {attr_id}, {r.Value});");
                    AppSettings.AppSettings.guestContext.Database.ExecuteSqlInterpolated(query);

                }
            }

            if (!string.IsNullOrEmpty(comment))
            {
                var query = FormattableStringFactory.Create($@"INSERT INTO property_comment (booking_id, comment) " +
                    $"VALUES ({id}, '{comment}');");

                AppSettings.AppSettings.guestContext.Database.ExecuteSqlInterpolated(query);
            }


            return RedirectToAction("Bookings");
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
