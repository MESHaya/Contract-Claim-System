using ClaimSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text.pdf;
using iTextSharp.text;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Data;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Security.Claims;
using ClaimSystem.Controllers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace ClaimSystem.Models
{
    public class ClaimDbContext
    {
        private static readonly string constr = "server=localhost;uid=root;pwd=Password1234567890;database=claimdb";

        private readonly ILogger<ClaimDbContext> _logger;

        public ClaimDbContext(ILogger<ClaimDbContext> logger)
        {
            _logger = logger;
        }

        MySqlConnection connection = null;
        private async Task<User> GetUserByUsername(string username)
        {
            try
            {
                // Log the connection attempt
                _logger.LogInformation("Attempting to connect to the database...");

                await connection.OpenAsync();  // Try to open the connection

                // Log success if the connection opens
                _logger.LogInformation("Successfully connected to the database.");

                using (var connection = new MySqlConnection(constr))
                {
                    await connection.OpenAsync();

                    string query = @"
    SELECT UserId, Username, Password, Role 
    FROM Users 
    WHERE Username = @Username;";


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
                _logger.LogError(ex, "Error retrieving user by username.");
            }

            return null;
        }


        public static bool CreateClaim(Claims claim)
        {
            try
            {
                using MySqlConnection con = new(constr);
                con.Open();

                string qry = @"INSERT INTO Claims (StatusId, LecturerName, HoursWorked, HourlyRate, Notes, Status, FileName, DateSubmitted, RejectionReason) 
                       VALUES (@statusId, @lecturerName, @hoursWorked, @hourlyRate, @notes, @status, @fileName, @dateSubmitted, @rejectionReason)";

                using MySqlCommand cmd = new(qry, con);
                cmd.Parameters.AddWithValue("@statusId", claim.StatusId);
                cmd.Parameters.AddWithValue("@lecturerName", claim.LecturerName);
                cmd.Parameters.AddWithValue("@hoursWorked", claim.HoursWorked);
                cmd.Parameters.AddWithValue("@hourlyRate", claim.HourlyRate);
                cmd.Parameters.AddWithValue("@notes", claim.Notes ?? (object)DBNull.Value); // Handle optional fields
                cmd.Parameters.AddWithValue("@status", claim.Status);
                cmd.Parameters.AddWithValue("@fileName", claim.FileName ?? (object)DBNull.Value); // Handle optional fields
                cmd.Parameters.AddWithValue("@dateSubmitted", claim.DateSubmitted);
                cmd.Parameters.AddWithValue("@rejectionReason", claim.RejectionReason ?? (object)DBNull.Value); // Handle optional fields

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }

        public static List<Claims> GetAllClaims()
        {
            List<Claims> claimsList = new();

            try
            {
                using MySqlConnection con = new(constr);
                con.Open();

                string qry = @"SELECT Id, StatusId, LecturerName, HoursWorked, HourlyRate, Notes, Status, FileName, DateSubmitted, RejectionReason FROM Claims";
                using MySqlCommand cmd = new(qry, con);

                using MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Claims claim = new()
                    {
                        Id = reader.GetInt32("Id"),
                        StatusId = reader.GetInt32("StatusId"),
                        LecturerName = reader.GetString("LecturerName"),
                        HoursWorked = reader.GetDecimal("HoursWorked"),
                        HourlyRate = reader.GetDecimal("HourlyRate"),
                        Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString("Notes"),
                        Status = reader.GetString("Status"),
                        FileName = reader.IsDBNull(reader.GetOrdinal("FileName")) ? null : reader.GetString("FileName"),
                        DateSubmitted = reader.GetDateTime("DateSubmitted"),
                        RejectionReason = reader.IsDBNull(reader.GetOrdinal("RejectionReason")) ? null : reader.GetString("RejectionReason")
                    };

                    claimsList.Add(claim);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return claimsList;
        }

        public static List<Lecturer> GetAllLecturers()
        {
            List<Lecturer> lecturersList = new List<Lecturer>();

            try
            {
                using MySqlConnection con = new MySqlConnection(constr);
                con.Open();

                // Query to get all lecturers
                string qry = @"SELECT LecturerID, Name, DateOfHire, Username, Password, Email, Phone, Department
                       FROM Lecturers";

                using MySqlCommand cmd = new MySqlCommand(qry, con);

                using MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Lecturer lecturer = new Lecturer
                    {
                        LecturerId = reader.GetInt32("LecturerID"),
                        Name = reader.GetString("Name"),
                        Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                        Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString("Phone"),
                        Department = reader.IsDBNull(reader.GetOrdinal("Department")) ? null : reader.GetString("Department"),
                        DateOfHire = reader.GetDateTime("DateOfHire"),
                        Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? null : reader.GetString("Username"),
                        Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? null : reader.GetString("Password"),
                     
                    };

                    lecturersList.Add(lecturer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                // Optionally, log the full stack trace for more detail
                Console.WriteLine(ex.StackTrace);
            }

            return lecturersList;
        }

        public static bool EditLecturer(Lecturer lecturer)
        {
            try
            {
                using MySqlConnection con = new(constr);
                con.Open();

                string qry = @"
            UPDATE Lecturers 
            SET 
                Name = @name,
                Email = @email,
                Phone = @phone,
                Department = @department,
                DateOfHire = @dateOfHire,
                Username = @username,
                Password = @password
            WHERE LecturerID = @lecturerId";

                using MySqlCommand cmd = new(qry, con);
                cmd.Parameters.AddWithValue("@lecturerId", lecturer.LecturerId);
                cmd.Parameters.AddWithValue("@name", lecturer.Name);
                cmd.Parameters.AddWithValue("@email", lecturer.Email ?? (object)DBNull.Value); // Handle optional fields
                cmd.Parameters.AddWithValue("@phone", lecturer.Phone ?? (object)DBNull.Value); // Handle optional fields
                cmd.Parameters.AddWithValue("@department", lecturer.Department ?? (object)DBNull.Value); // Handle optional fields
                cmd.Parameters.AddWithValue("@dateOfHire", lecturer.DateOfHire);
                cmd.Parameters.AddWithValue("@username", lecturer.Username ?? (object)DBNull.Value); // Handle optional fields
                cmd.Parameters.AddWithValue("@password", lecturer.Password ?? (object)DBNull.Value); // Handle optional fields

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }

        public static Lecturer GetLecturerById(int lecturerId)
        {
            Lecturer lecturer = null;

            try
            {
                using MySqlConnection con = new(constr);
                con.Open();

                string qry = @"SELECT LecturerID, Name, DateOfHire, Username, Password, Email, Phone, Department
                       FROM Lecturers 
                       WHERE LecturerID = @lecturerId";

                using MySqlCommand cmd = new(qry, con);
                cmd.Parameters.AddWithValue("@lecturerId", lecturerId);

                using MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lecturer = new Lecturer
                    {
                        LecturerId = reader.GetInt32("LecturerID"),
                        Name = reader.GetString("Name"),
                        Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                        Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString("Phone"),
                        Department = reader.IsDBNull(reader.GetOrdinal("Department")) ? null : reader.GetString("Department"),
                        DateOfHire = reader.GetDateTime("DateOfHire"),
                        Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? null : reader.GetString("Username"),
                        Password = reader.IsDBNull(reader.GetOrdinal("Password")) ? null : reader.GetString("Password"),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return lecturer;
        }
        public static bool SaveChanges(Lecturer lecturer)
        {
            try
            {
                return EditLecturer(lecturer); // Reuse the existing EditLecturer method to save changes
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while saving changes: " + ex.Message);
                return false;
            }
        }

        public static List<Claims> GetClaimsForUser(int userId)
            {
                List<Claims> claimsList = new();

                try
                {
                    using MySqlConnection con = new(constr);
                    con.Open();

                    string qry = @"SELECT Id, StatusId, LecturerName, HoursWorked, HourlyRate, Notes, Status, FileName, DateSubmitted, RejectionReason 
                       FROM Claims WHERE UserId = @userId"; // Adjust based on your actual table structure

                    using MySqlCommand cmd = new(qry, con);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Claims claim = new()
                        {
                            Id = reader.GetInt32("Id"),
                            StatusId = reader.GetInt32("StatusId"),
                            LecturerName = reader.GetString("LecturerName"),
                            HoursWorked = reader.GetDecimal("HoursWorked"),
                            HourlyRate = reader.GetDecimal("HourlyRate"),
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString("Notes"),
                            Status = reader.GetString("Status"),
                            FileName = reader.IsDBNull(reader.GetOrdinal("FileName")) ? null : reader.GetString("FileName"),
                            DateSubmitted = reader.GetDateTime("DateSubmitted"),
                            RejectionReason = reader.IsDBNull(reader.GetOrdinal("RejectionReason")) ? null : reader.GetString("RejectionReason")
                        };

                        claimsList.Add(claim);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }

                return claimsList;
            }
            public static bool UpdateClaim(Claims claim)
            {
                try
                {
                    using MySqlConnection con = new(constr);
                    con.Open();

                    string qry = @"UPDATE Claims SET 
                        Status = @status, 
                        RejectionReason = @rejectionReason 
                        WHERE Id = @id";

                    using MySqlCommand cmd = new(qry, con);
                    cmd.Parameters.AddWithValue("@id", claim.Id);
                    cmd.Parameters.AddWithValue("@status", claim.Status);
                    cmd.Parameters.AddWithValue("@rejectionReason", claim.RejectionReason ?? (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return false;
                }
            }

        public static List<Claims> GetApprovedClaimsByDateRange(DateTime startDate, DateTime endDate)
        {
            List<Claims> claimsList = new List<Claims>();

            try
            {
                using MySqlConnection con = new MySqlConnection(constr);
                con.Open();

                // Query to get approved claims within the specified date range
                string qry = @"SELECT Id, StatusId, LecturerName, HoursWorked, HourlyRate, Notes, Status, FileName, DateSubmitted, RejectionReason
                       FROM Claims
                       WHERE Status = 'Approved' 
                       AND DateSubmitted BETWEEN @startDate AND @endDate";

                using MySqlCommand cmd = new MySqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);

                using MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Claims claim = new Claims
                    {
                        Id = reader.GetInt32("Id"),
                        StatusId = reader.GetInt32("StatusId"),
                        LecturerName = reader.GetString("LecturerName"),
                        HoursWorked = reader.GetDecimal("HoursWorked"),
                        HourlyRate = reader.GetDecimal("HourlyRate"),
                        Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString("Notes"),
                        Status = reader.GetString("Status"),
                        FileName = reader.IsDBNull(reader.GetOrdinal("FileName")) ? null : reader.GetString("FileName"),
                        DateSubmitted = reader.GetDateTime("DateSubmitted"),
                        RejectionReason = reader.IsDBNull(reader.GetOrdinal("RejectionReason")) ? null : reader.GetString("RejectionReason")
                    };

                    claimsList.Add(claim);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return claimsList;
        }

        public static bool SaveReport(string reportContent, string filePath)
        {
            try
            {
                // Here, we will write the content to a file on disk. If you want to store it in a database, you can adjust this to insert into a table
                File.WriteAllText(filePath, reportContent); // Save as text file (you can change to PDF saving logic as needed)
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while saving the report: " + ex.Message);
                return false;
            }
        }

        internal static void SaveReport(Report report)
        {
            throw new NotImplementedException();
        }
    }
}


        



    


