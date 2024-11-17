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

        public static Lecturer GetLecturerDetails(int id)
        {
            try
            {
                using MySqlConnection con = new(constr);
                con.Open();

                string qry = @"SELECT Id, Name, ContactInfo, DateOfHire FROM Lecturers WHERE Id = @id";
                using MySqlCommand cmd = new(qry, con);
                cmd.Parameters.AddWithValue("@id", id);

                using MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                Lecturer lecturer = new()
                {
                    LecturerId = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    ContactInfo = reader.GetString("ContactInfo"),
                    DateOfHire = reader.GetDateTime("DateOfHire")
                };

                return lecturer;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return null;
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



    }
}

