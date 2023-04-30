using DailyApartmentsMVC.Models.OwnerModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace DailyApartmentsMVC.Controllers
{
    [Authorize]
    public class PropertyOwnerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddProperty()
        {
            ViewBag.Attributes = AppSettings.AppSettings.ownerContext.TermsAttributes.ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProperty(AddProperty model, List<int> attribute)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                // Return the view with validation errors
                return View(model);
            }

            List<string> imageUrl = new List<string>();

            foreach (var photo in model.Photo)
            {
                if (model.Photo != null)
                {
                    using var httpClient = new HttpClient();
                    using var content = new MultipartFormDataContent();
                    content.Add(new StringContent("1ccfac3ffddc022d212e2a5617ba363f"), "key");

                    using var imageStream = photo.OpenReadStream();
                    content.Add(new StreamContent(imageStream), "image", photo.FileName);

                    var response = await httpClient.PostAsync("https://api.imgbb.com/1/upload", content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

                        imageUrl.Add($"{jsonResponse.data.url}");
                    }
                    else
                    {
                        Console.WriteLine($"API request failed with status code {response.StatusCode}");
                    }
                }
            }


            var commandText = "CALL sp_add_property(@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11)";

            await AppSettings.AppSettings.ownerContext?.Database.ExecuteSqlRawAsync(commandText,
                new object[] {
                    model.Title, model.Description, model.RoomNumber, model.SleepingPlaceNumber,
                    imageUrl, model.Price, model.Country, model.City, model.Street, model.House,
                    model.Type, model.MinRentalDays
                });

            int id = AppSettings.AppSettings.ownerContext.PropertyListOwners.Max(x => x.Id) ?? -1;
            if(id != -1 && attribute.Any())
            {
                foreach (var attr in attribute)
                {
                    AppSettings.AppSettings.ownerContext.Database
                        .ExecuteSqlRawAsync("CALL insert_additional_term(@p0, @p1, @p2);", id, attr, true);
                }
            }

            // Redirect to a success page
            ViewBag.PropertyAdded = true;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditProperty(int id)
        {
            var property = AppSettings.AppSettings.ownerContext.PropertyListOwners.FirstOrDefault(p => p.Id == id);

            var model = new EditProperty
            {
                Id = id,
                Title = property?.Title,
                Description = property?.Description,
                RoomNumber = property?.RoomNumber ?? 1,
                Price = property?.Price ?? 0,
                SleepingPlaceNumber = property?.SleepingPlaceNumber ?? 1,
                Country = property?.Country,
                City = property?.City,
                Street = property?.Street,
                House = property?.House ?? 1,
                Type = property?.Type ?? "Квартира",
                MinRentalDays = property?.MinRentalDays ?? 1
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProperty(EditProperty property, int id)
        {

            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                // Return the view with validation errors
                return View(property);
            }


            List<string> imageUrl = new List<string>();

            if (property.Photo != null)
            {
                foreach (var photo in property.Photo)
                {
                    if (property.Photo != null)
                    {
                        using var httpClient = new HttpClient();
                        using var content = new MultipartFormDataContent();
                        content.Add(new StringContent("1ccfac3ffddc022d212e2a5617ba363f"), "key");

                        using var imageStream = photo.OpenReadStream();
                        content.Add(new StreamContent(imageStream), "image", photo.FileName);

                        var response = await httpClient.PostAsync("https://api.imgbb.com/1/upload", content);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseContent = await response.Content.ReadAsStringAsync();
                            dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

                            imageUrl.Add($"{jsonResponse.data.url}");
                        }
                        else
                        {
                            Console.WriteLine($"API request failed with status code {response.StatusCode}");
                        }
                    }
                }
            }

            var commandText = "CALL sp_update_property(@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12)";

            if (AppSettings.AppSettings.ownerContext != null)
            {
                AppSettings.AppSettings.ownerContext.Database.ExecuteSqlRaw(commandText,
                    new object[] {
                    property.Id, property.Title, property.Description, property.RoomNumber,
                    property.SleepingPlaceNumber, property.Price, property.Country, property.City,
                    property.Street, property.House, property.Type, property.MinRentalDays, imageUrl.Count() > 0 ? imageUrl : null
                    });
            }

            TempData["Edited"] = true;

            return RedirectToAction("ListProperties");
        }

        [HttpGet]
        public async Task<IActionResult> ListProperties()
        {
            var model = AppSettings.AppSettings.ownerContext.PropertyListOwners.ToList();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> DeactivateProperty(int id)
        {
            FormattableString query = FormattableStringFactory.Create($"SELECT deactivate_property({id});");
            await AppSettings.AppSettings.ownerContext.Database.ExecuteSqlInterpolatedAsync(query);
            TempData["Deactivated"] = true;
            return RedirectToAction("ListProperties");
        }

        [HttpPost]
        public async Task<IActionResult> ActivateProperty(int id)
        {
            FormattableString query = FormattableStringFactory.Create($"SELECT activate_property({id});");
            await AppSettings.AppSettings.ownerContext.Database.ExecuteSqlInterpolatedAsync(query);
            TempData["Deactivated"] = false;
            return RedirectToAction("ListProperties");
        }

        [HttpPost]
        public async Task<IActionResult> SendReview(int id, Dictionary<int, int> rates, string comment)
        {
            if (!string.IsNullOrEmpty(comment))
            {
                var query = FormattableStringFactory.Create($@"INSERT INTO client_comment (booking_id, comment) " +
                    $"VALUES ({id}, '{comment}');");

                await AppSettings.AppSettings.ownerContext.Database.ExecuteSqlInterpolatedAsync(query);
            }

            if (rates.Count() > 0)
            {
                var query = FormattableStringFactory.Create($@"INSERT INTO client_review (booking_id, rate) " +
                    $"VALUES ({id}, '{rates[id]}');");

                await AppSettings.AppSettings.ownerContext.Database.ExecuteSqlInterpolatedAsync(query);
            }

            return RedirectToAction("ListBookings");
        }


        [HttpGet]
        public async Task<IActionResult> ListBookings(int id = -1, string status = "all")
        {
            var bookings = AppSettings.AppSettings.ownerContext.BookingsForOwners.ToList();
            var myRates = AppSettings.AppSettings.ownerContext.MyRatesAndCommentsForOwners.ToList();

            ViewBag.PropertyId = id;
            ViewBag.MyRates = myRates;

            var statusSelect = new List<SelectListItem>
            {
                new SelectListItem {Value = "new", Text = "Нові"},
                new SelectListItem {Value = "confirmed", Text = "Підтверджені"},
                new SelectListItem {Value = "cancelled", Text = "Скасовані"},
                new SelectListItem {Value = "all", Text = "Усі"},
            };

            ViewBag.Status = statusSelect;
            ViewBag.SelectedStatus = status;

            return View(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var query = FormattableStringFactory.Create($"SELECT cancel_booking({id});");
            await AppSettings.AppSettings.ownerContext.Database.ExecuteSqlInterpolatedAsync(query);
            TempData["BookingCanceled"] = true;
            return RedirectToAction("ListBookings");
        }


        [HttpPost]
        public async Task<IActionResult> ApproveBooking(int id)
        {
            var query = FormattableStringFactory.Create($"SELECT approve_booking({id});");
            await AppSettings.AppSettings.ownerContext.Database.ExecuteSqlInterpolatedAsync(query);
            TempData["BookingApproved"] = true;
            return RedirectToAction("ListBookings");
        }

        [HttpGet]
        public async Task<IActionResult> Statistics(string propertyId = "-1", string period = "-1", DateTime? date = null)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }


            var dateParameter = new NpgsqlParameter("date_param", NpgsqlDbType.Date)
            {
                Value = date
            };


            int? propId = propertyId == "-1" ? null : int.Parse(propertyId);
            int? per = period == "-1" ? null : int.Parse(period);

            var propertyList = AppSettings.AppSettings.ownerContext.OwnerBookingsStatistics.Select(b => new
            {
                Title = b.Title ?? "",
                Id = b.PropertyId ?? -1
            }).Distinct().ToList();
            

            var responseMonthlyIncome = AppSettings.AppSettings.ownerContext.MonthlyIncomes.FromSqlRaw(
                "SELECT * FROM owner_monthly_income(@p0, @p1, @date_param)", propId, per, dateParameter)
                .ToList();

            var monthlyIncome = responseMonthlyIncome
                .Select(i => new
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse( i.MonthIndex )),
                    Value = i.Value
                })
                .ToList();




            var responseMonthlyBookings = AppSettings.AppSettings.ownerContext.MonthlyIncomes.FromSqlRaw(
                "SELECT * FROM owner_monthly_bookings(@p0, @p1, @date_param)", propId, per, dateParameter)
                .ToList();


            var monthlyBookings = responseMonthlyBookings
                .Select(i => new
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(i.MonthIndex)),
                    Value = i.Value
                })
                .ToList();

            while(monthlyIncome.Count() < (per ?? -1))
            {
                string? firstMonth;
                if (monthlyIncome.Count() > 0)
                {
                    firstMonth = monthlyIncome.First().Month;
                }
                else
                {
                    firstMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                }

                DateTime prevMonth = DateTime.ParseExact(firstMonth, "MMMM", CultureInfo.GetCultureInfo("uk-UA")).AddMonths(-1);

                monthlyIncome.Insert(0, new
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(prevMonth.Month),
                    Value = 0m,
                });
            }

            while (monthlyBookings.Count() < (per ?? -1))
            {
                string? firstMonth;
                if (monthlyBookings.Count() > 0)
                {
                    firstMonth = monthlyBookings.First().Month;
                }
                else
                {
                    firstMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                }

                DateTime prevMonth = DateTime.ParseExact(firstMonth, "MMMM", CultureInfo.GetCultureInfo("uk-UA")).AddMonths(-1);

                monthlyBookings.Insert(0, new
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(prevMonth.Month),
                    Value = 0m,
                });
            }


            ViewBag.MonthlyIncome = monthlyIncome;
            ViewBag.MonthlyBookings = monthlyBookings;


            // PROPERTY INCOME RATIO
            var ratioIncome = propertyList.Select(p => new
            {
                Title = p.Title,
                Id = p.Id,
                Value = AppSettings.AppSettings.ownerContext.MonthlyIncomes
                .FromSqlRaw("SELECT * FROM owner_monthly_income(@p0, @p1, @date_param)", p.Id, per, dateParameter).Sum(x => x.Value)
            }).ToList();

            ViewBag.RatioIncome = ratioIncome;


            // PROPERTY BOOKINGS RATIO
            var ratioBookings = propertyList.Select(p => new
            {
                Title = p.Title,
                Id = p.Id,
                Value = AppSettings.AppSettings.ownerContext.MonthlyIncomes.FromSqlRaw(
                "SELECT * FROM owner_monthly_bookings(@p0, @p1, @date_param)", p.Id, per, dateParameter)
                .Sum(x => x.Value)
            }).ToList();

            ViewBag.RatioBookings = ratioBookings;


            var periodSelect = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = "1 місяць"},
                new SelectListItem {Value = "3", Text = "3 місяці"},
                new SelectListItem {Value = "6", Text = "6 місяців"},
                new SelectListItem {Value = "12", Text = "1 рік"},
                new SelectListItem {Value = "-1", Text = "Увесь час"},
            };

            var propertySelect = new List<SelectListItem>();
            foreach (var p in propertyList)
                propertySelect.Add(new SelectListItem { Value = $"{p.Id}", Text = p.Title });
            propertySelect.Add(new SelectListItem { Value = "-1", Text = "Усі оголошення" });

            ViewBag.Period = periodSelect;
            ViewBag.SelectedPeriod = period;

            ViewBag.Date = date;

            ViewBag.Property = propertySelect;
            ViewBag.SelectedProperty = propertyId;
            

            return View();
        }
    }
}
