using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using VodokanalWeb.Models;
using Microsoft.AspNetCore.Http;

namespace VodokanalWeb.Controllers
{
    public class MonitoringController(IConfiguration configuration) : Controller
    {
        private readonly IConfiguration _configuration = configuration;

        [HttpGet]
        public IActionResult DatabaseHealth()
        {
            var model = new DatabaseHealthModel();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    connection.Open();

                 
                    var command = new SqlCommand(
                        "SELECT " +
                        "ISNULL((SELECT COUNT(*) FROM Subscribers), 0) as SubscribersCount, " +
                        "ISNULL((SELECT COUNT(*) FROM Accruals WHERE Status = 'Pending'), 0) as PendingAccruals, " +
                        "ISNULL((SELECT DATEDIFF(MINUTE, MAX(LogDate), GETDATE()) FROM SystemLogs), 999) as LastLogMinutes, " +
                        "ISNULL((SELECT COUNT(*) FROM sys.dm_exec_sessions), 0) as ActiveConnections",
                        connection);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model.SubscribersCount = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            model.PendingAccruals = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            model.LastLogMinutes = reader.IsDBNull(2) ? 999 : reader.GetInt32(2);
                            model.ActiveConnections = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                            model.IsHealthy = true;
                        }
                    }
                    command = new SqlCommand(
                        "SELECT ISNULL(SUM(size * 8.0 / 1024), 0) as SizeMB FROM sys.database_files",
                        connection);
                    var result = command.ExecuteScalar();
                    model.DatabaseSizeMB = result == DBNull.Value ? 0 : Convert.ToDecimal(result);

                    command = new SqlCommand(
                        "SELECT TOP 10 LogDate, Message, Source FROM SystemLogs WHERE LogLevel = 'Error' ORDER BY LogDate DESC",
                        connection);

                    var errors = new List<SystemLog>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            errors.Add(new SystemLog
                            {
                                LogDate = reader.IsDBNull(0) ? DateTime.Now : Convert.ToDateTime(reader["LogDate"]),
                                Message = reader.IsDBNull(1) ? string.Empty : reader["Message"].ToString() ?? string.Empty,
                                Source = reader.IsDBNull(2) ? string.Empty : reader["Source"].ToString() ?? string.Empty
                            });
                        }
                    }
                    model.RecentErrors = errors;
                }
                catch (Exception ex)
                {
                    model.IsHealthy = false;
                    model.ErrorMessage = ex.Message;
                }
            }

            return View(model);
        }
    }

    public class DatabaseHealthModel
    {
        public bool IsHealthy { get; set; }
        public int SubscribersCount { get; set; }
        public int PendingAccruals { get; set; }
        public int LastLogMinutes { get; set; }
        public int ActiveConnections { get; set; }
        public decimal DatabaseSizeMB { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public List<SystemLog> RecentErrors { get; set; } = [];
    }
}
