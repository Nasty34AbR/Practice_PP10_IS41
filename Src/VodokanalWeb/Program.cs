using Microsoft.EntityFrameworkCore;
using VodokanalWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; 
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

builder.Services.AddAuthorization();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "subscribers",
    pattern: "Subscribers/{action=Index}/{id?}",
    defaults: new { controller = "Subscribers" });

app.MapControllerRoute(
    name: "account",
    pattern: "Account/{action=Login}",
    defaults: new { controller = "Account" });

app.MapControllerRoute(
    name: "about",
    pattern: "about",
    defaults: new { controller = "Home", action = "About" });

app.MapControllerRoute(
    name: "contacts",
    pattern: "contacts",
    defaults: new { controller = "Home", action = "Contacts" });


    app.MapGet("/test/profile", async (DatabaseContext context) =>
    {
        var userCount = await context.Users.CountAsync();
        return $"Users in database: {userCount}";
    });

    app.MapGet("/check-auth", (HttpContext context) =>
    {
        var result = new
        {
            IsAuthenticated = context.User.Identity?.IsAuthenticated ?? false,
            UserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            Username = context.User.Identity?.Name,
            SessionUserId = context.Session.GetInt32("UserId")
        };
        return Results.Json(result);
    });

app.Run();
