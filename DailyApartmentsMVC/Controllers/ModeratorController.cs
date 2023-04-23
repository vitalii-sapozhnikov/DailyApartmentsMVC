using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DailyApartmentsMVC.Controllers
{
    public class ModeratorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListProperties()
        {
            var model = AppSettings.AppSettings.moderatorContext.PropertiesForModerators.ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DenyPublication(int id)
        {
            var result = await AppSettings.AppSettings.moderatorContext.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT update_property_verification_status({id}, {false})");

            return RedirectToAction("ListProperties");
        }

        [HttpPost]
        public async Task<IActionResult> ApprovePublication(int id)
        {
            var result = await AppSettings.AppSettings.moderatorContext.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT update_property_verification_status({id}, {true})");

            return RedirectToAction("ListProperties");
        }
    }
}
