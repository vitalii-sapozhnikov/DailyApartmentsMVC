using DailyApartmentsMVC.Models;
using DailyApartmentsMVC.Models.OwnerModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.IO;
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProperty(AddProperty model)
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

            AppSettings.AppSettings.ownerContext?.Database.ExecuteSqlRaw(commandText,
                new object[] {
                    model.Title, model.Description, model.RoomNumber, model.SleepingPlaceNumber,
                    imageUrl, model.Price, model.Country, model.City, model.Street, model.House,
                    model.Type, model.MinRentalDays
                });


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
        public async Task<IActionResult> SendReview(int id, int rating, string comment)
        {
            if (!string.IsNullOrEmpty(comment))
            {
                var query = FormattableStringFactory.Create($@"INSERT INTO client_comment (booking_id, comment) " +
                    $"VALUES ({id}, '{comment}');");

                await AppSettings.AppSettings.guestContext.Database.ExecuteSqlInterpolatedAsync(query);
            }

            if (!string.IsNullOrEmpty(comment))
            {
                var query = FormattableStringFactory.Create($@"INSERT INTO client_review (booking_id, rate) " +
                    $"VALUES ({id}, '{rating}');");

                await AppSettings.AppSettings.guestContext.Database.ExecuteSqlInterpolatedAsync(query);
            }

            return RedirectToAction();
        }


        [HttpGet]
        public async Task<IActionResult> ListBookings(int id)
        {
            var bookings = AppSettings.AppSettings.ownerContext.BookingsForOwners.ToList();

            ViewBag.PropertyId = id;


            return View(bookings);
        }

    }
}
