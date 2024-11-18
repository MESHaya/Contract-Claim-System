using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ClaimSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        // Database connection string (adjust if necessary)
        private static readonly string constr = "server=localhost;uid=root;pwd=Password1234567890;database=claimdb";

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        // Show login page
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await GetUserByUsername(username);

            if (user == null)
            {
                // Username does not exist
                ModelState.AddModelError(string.Empty, "The username does not exist.");
                return View();
            }

            // Verify the password (you should ideally hash passwords in the database and compare hashes)
            if (user.Password != password)
            {
                ModelState.AddModelError(string.Empty, "The password is incorrect.");
                return View();
            }

            // Check user roles and redirect accordingly
            var role = await GetUserRole(user.UserId);

            // Check user roles and redirect accordingly
            if (user.Role == "Lecturer")
            {
                return RedirectToAction("LecturerDash", "Lecturer");
            }
            else if (user.Role == "Academic Manager")
            {
                return RedirectToAction("Academic_Manager_Dash", "Manager");
            }
            else if (user.Role == "Programme Coordinator")
            {
                return RedirectToAction("Programme_Coordinator_Dash", "Programme");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Role not recognized.");
                return View();
            }

        }

        // Handle logout
        public IActionResult Logout()
        {
            // Clear any session data or authentication cookies if used
            return RedirectToAction("Login", "Account");
        }

        // Retrieve user by username
        private async Task<User> GetUserByUsername(string username)
        {
            try
            {
                using (var connection = new MySqlConnection(constr))
                {
                    // Log the connection attempt
                    _logger.LogInformation("Attempting to connect to the database...");

                    await connection.OpenAsync();  // Try to open the connection

                    // Log success if the connection opens
                    _logger.LogInformation("Successfully connected to the database.");

                    string query = @"
                        SELECT LecturerID AS Id, Name, Username, Password, 'Lecturer' AS Role FROM Lecturers WHERE Username = @Username
                        UNION
                        SELECT MangerId AS Id, Name, Username, Password, 'Academic Manager' AS Role FROM AcademicManger WHERE Username = @Username
                        UNION
                        SELECT CoordinatorsId AS Id, Name, Username, Password, 'Programme Coordinator' AS Role FROM Coordinators WHERE Username = @Username";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new User
                                {
                                    UserId = reader.GetInt32("Id"),
                                    Username = reader.GetString("Username"),
                                    Password = reader.GetString("Password"),
                                    Role = reader.GetString("Role")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log connection error
                _logger.LogError(ex, "Error retrieving user by username.");
                ModelState.AddModelError(string.Empty, "Error connecting to the database. Please try again later.");
            }

            // Return null if no user is found or an error occurs
            return null;
        }

        // Retrieve role for the user (already part of the GetUserByUsername query)
        private async Task<string> GetUserRole(int userId)
        {
            // The role is returned in the GetUserByUsername method, so we directly use that value
            return userId != 0 ? userId.ToString() : null;
        }
    }

    // Custom User class to hold user data
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Ideally, store a hashed password
        public string Role { get; set; } // Added role for checking
    }
}
