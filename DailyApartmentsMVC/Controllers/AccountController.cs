using DailyApartmentsMVC.Models.GuestModel;
using DailyApartmentsMVC.Models.RegistrationModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Configuration;
using System.Security.Claims;
using System.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using DailyApartmentsMVC.Models.OwnerModel;

namespace DailyApartmentsMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IServiceProvider _serviceProvider;
        private GuestContext _guestContext;

        public AccountController(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            this.configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            [Bind("UserName, Email, Password, Name, Surname, DateOfBirth")] GuestRegistration guestRegistration,
            [Bind("OwnerUserName, OwnerEmail, OwnerPassword, OwnerName, OwnerSurname, OwnerPassportID, OwnerTaxNumber, OwnerPhoneNumber")] PropertyOwnerRegistration ownerRegistration,
            bool isPropertyOwner)
        {

            if (isPropertyOwner)
            {
                if (ownerRegistration.OwnerUserName != null)
                {
                    using (var connection = new NpgsqlConnection(configuration.GetConnectionString("UserManager")))
                    {
                        await connection.OpenAsync();
                        using (var command = new NpgsqlCommand())
                        {
                            command.Connection = connection;

                            command.CommandText = $"CALL sp_create_property_owner_user('{ownerRegistration.OwnerUserName}', " +
                                $"'{ownerRegistration.OwnerEmail}', '{ownerRegistration.OwnerPassword}', '{ownerRegistration.OwnerName}', " +
                                $"'{ownerRegistration.OwnerSurname}', '{ownerRegistration.OwnerPassportID}', '{ownerRegistration.OwnerTaxNumber}', " +
                                $"'{ownerRegistration.OwnerPhoneNumber}');";

                            await command.ExecuteNonQueryAsync();
                        }
                    }
                    return RedirectToAction("Index", "PropertyOwner");
                }
            }
            else
            {
                if (guestRegistration.UserName != null)
                {
                    using (var connection = new NpgsqlConnection(configuration.GetConnectionString("UserManager")))
                    {
                        await connection.OpenAsync();
                        using (var command = new NpgsqlCommand())
                        {
                            command.Connection = connection;

                            command.CommandText = $"CALL sp_create_guest_user('{guestRegistration.UserName}', " +
                                $"'{guestRegistration.Email}', '{guestRegistration.Password}', '{guestRegistration.Name}', " +
                                $"'{guestRegistration.Surname}', '{guestRegistration.DateOfBirth.ToString("yyyy-MM-dd")}');\r\n";

                            await command.ExecuteNonQueryAsync();
                        }
                    }
                    return RedirectToAction("Index", "Guest");
                }
            }
            return View();

        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, bool isPropertyOwner)
        {
            if (isPropertyOwner)
            {
                username += "_owner";

                string connectionString = $"host=localhost;database=airbnb;port=5432;username={username};password={password}";

                try
                {
                    var connection = new NpgsqlConnection(connectionString);
                    await connection.OpenAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Невірний логін або пароль");
                    return View();
                }

                DbContextOptions<OwnerContext> options = new DbContextOptionsBuilder<OwnerContext>()
                    .UseNpgsql(connectionString).Options;

                AppSettings.AppSettings.ownerContext = new OwnerContext(options);


                var identity = new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.Name, username),
                            new Claim(ClaimTypes.Role, "propertyOwner")
                        }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();

                // Sign in user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

                return RedirectToAction("Index", "PropertyOwner");
            }
            else
            {
                username += "_guest";

                string connectionString = $"host=localhost;database=airbnb;port=5432;username={username};password={password}";

                try
                {
                    var connection = new NpgsqlConnection(connectionString);
                    await connection.OpenAsync();
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "Невірний логін або пароль");
                    return View();
                }

                //DbContextOptions<GuestContext> options = new DbContextOptionsBuilder<GuestContext>()
                //    .UseNpgsql(connectionString).Options;


                //guestContext = new GuestContext(options);

                //guestContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;


                DbContextOptions<GuestContext> options = new DbContextOptionsBuilder<GuestContext>()
                    .UseNpgsql(connectionString).Options;

                AppSettings.AppSettings.guestContext = new GuestContext(options);





                var identity = new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.Name, username),
                            new Claim(ClaimTypes.Role, "guest")
                        }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();

                // Sign in user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

                return RedirectToAction("Index", "Guest");
            }

            // If user does not exist or password is incorrect, display error message
            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }
    }
}