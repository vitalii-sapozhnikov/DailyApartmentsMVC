using DailyApartmentsMVC.Models.GuestModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<AirbnbContext>(options => options.UseNpgsql(
//        builder.Configuration.GetConnectionString("AirbnbAdmin")
//    ));

//builder.Services.AddDbContext<GuestContext>();
builder.Services.AddSingleton<GuestContext>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.Name = "AuthCookies";
        options.Cookie.HttpOnly = false;
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PropertyOwnerPolicy", policy =>
    {
        policy.RequireClaim("UserRole", "PropertyOwner");
    });

    options.AddPolicy("GuestPolicy", policy =>
    {
        policy.RequireClaim("UserRole", "Guest");
    });
});

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=PropertyOwner}/{action=AddProperty}");

app.Run();
