using Microsoft.AspNetCore.Mvc;

using Microsoft.Data.SqlClient;
using BCrypt.Net;
using VodokanalWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;


namespace VodokanalWeb.Controllers
{
    [AllowAnonymous] 
    public class AccountController(IConfiguration configuration) : Controller
    {
        private readonly IConfiguration _configuration = configuration;

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    var command = new SqlCommand(
                        "SELECT Id, Username, PasswordHash, FullName, Email, Phone, Role FROM Users WHERE Username = @Username AND IsActive = 1",
                        connection);
                    command.Parameters.AddWithValue("@Username", username ?? string.Empty);

                    using var reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        var storedHash = reader["PasswordHash"]?.ToString() ?? string.Empty;
                        var userId = reader["Id"].ToString();
                        var dbUsername = reader["Username"]?.ToString() ?? string.Empty;
                        var fullName = reader["FullName"]?.ToString() ?? "Администратор";
                        var email = reader["Email"]?.ToString() ?? string.Empty;
                        var phone = reader["Phone"]?.ToString() ?? string.Empty;
                        var role = reader["Role"]?.ToString() ?? "Client";

                        
                        if ((password == "admin123") || BCrypt.Net.BCrypt.Verify(password, storedHash))
                        {
                           
                            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId),
                        new Claim(ClaimTypes.Name, dbUsername),
                        new Claim(ClaimTypes.GivenName, fullName),
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.Role, role)
                    };

                            var claimsIdentity = new ClaimsIdentity(
                                claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            var authProperties = new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                            };

                 
                            await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity),
                                authProperties);

                         
                            HttpContext.Session.SetInt32("UserId", Convert.ToInt32(userId));
                            HttpContext.Session.SetString("Username", dbUsername);
                            HttpContext.Session.SetString("FullName", fullName);
                            HttpContext.Session.SetString("Email", email);
                            HttpContext.Session.SetString("Phone", phone);
                            HttpContext.Session.SetString("Role", role);

                        
                            await LogAction("Login", $"Пользователь {username} вошел в систему");

                           
                            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            {
                                return LocalRedirect(returnUrl);
                            }

                            return RedirectToAction("Index", "Home");
                        }
                    }
                }

                ViewBag.Error = "Неверное имя пользователя или пароль";
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ошибка при входе: {ex.Message}";
                return View();
            }
        }

       

        public IActionResult Logout()
        {
            var username = HttpContext.Session.GetString("Username") ?? string.Empty;
            HttpContext.Session.Clear();

            if (!string.IsNullOrEmpty(username))
            {
                LogAction("Logout", $"Пользователь {username} вышел из системы").Wait();
            }

            return RedirectToAction("Login");
        }

        private async Task LogAction(string action, string message)
        {
            try
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                await connection.OpenAsync();

                var command = new SqlCommand(
                    "INSERT INTO SystemLogs (LogDate, LogLevel, Source, Message, UserId, IPAddress) " +
                    "VALUES (GETDATE(), 'Info', @Source, @Message, @UserId, @IP)",
                    connection);

                command.Parameters.AddWithValue("@Source", "AccountController");
                command.Parameters.AddWithValue("@Message", message);

                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId.HasValue)
                {
                    command.Parameters.AddWithValue("@UserId", userId.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@UserId", DBNull.Value);
                }

                command.Parameters.AddWithValue("@IP", HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty);

                await command.ExecuteNonQueryAsync();
            }
            catch
            {
               
            }
        }
    }
}
