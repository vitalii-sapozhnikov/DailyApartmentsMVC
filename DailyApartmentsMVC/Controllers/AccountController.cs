using DailyApartmentsMVC.Models.GuestModel;
using DailyApartmentsMVC.Models.RegistrationModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DailyApartmentsMVC.Models.OwnerModel;
using DailyApartmentsMVC.Models.ModeratorModel;

namespace DailyApartmentsMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public AccountController(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            this._configuration = configuration;
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
                    using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("UserManager")))
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
                    using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("UserManager")))
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
                    return RedirectToAction("Search", "Guest");
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
            string connectionString = $"host=localhost;database=airbnb;port=5432;username={username};password={password}";

            try
            {
                var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();
                DbContextOptions<ModeratorContext> options = new DbContextOptionsBuilder<ModeratorContext>()
                .UseNpgsql(connectionString).Options;

                AppSettings.AppSettings.moderatorContext = new ModeratorContext(options);


                var identity = new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.Name, username),
                            new Claim(ClaimTypes.Role, "moderator")
                        }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();

                // Sign in user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

                return RedirectToAction("Index", "Moderator");
            }
            catch (Exception)
            {
                // ignored
            }


            if (isPropertyOwner)
            {
                username += "_owner";

                connectionString = $"host=localhost;database=airbnb;port=5432;username={username};password={password}";

                try
                {
                    var connection = new NpgsqlConnection(connectionString);
                    await connection.OpenAsync();
                }
                catch (Exception)
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

                return RedirectToAction("ListProperties", "PropertyOwner");
            }
            else
            {
                username += "_guest";

                connectionString = $"host=localhost;database=airbnb;port=5432;username={username};password={password}";

                try
                {
                    var connection = new NpgsqlConnection(connectionString);
                    await connection.OpenAsync();
                }
                catch(Exception)
                {
                    ModelState.AddModelError("", "Невірний логін або пароль");
                    return View();
                }


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

                return RedirectToAction("Search", "Guest");
            }

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

    }
}