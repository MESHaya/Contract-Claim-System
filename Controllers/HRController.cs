using ClaimSystem.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace ClaimSystem.Controllers
{
    public class HRController : Controller
    {
        private readonly ILogger<HRController> _logger;
        // Database connection string (adjust if necessary)
        private static readonly string constr = "server=localhost;uid=root;pwd=Password1234567890;database=claimdb";

        public HRController(ILogger<HRController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HRdash()
        {
            return View();
        }

        public async Task<IActionResult> ViewAllLecturers()
        {
            try
            {
                var lecturers = ClaimDbContext.GetAllLecturers();  // Retrieve all lecturers from the database
                return View(lecturers);  // Pass the list of lecturers to the view
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all lecturers.");
                return View("Error");  // Handle errors by showing an error view
            }
        }

        public async Task<IActionResult> EditLecturer()
        {
            try
            {
                var lecturers = ClaimDbContext.GetAllLecturers();  // Get all lecturers from the database
                return View(lecturers);  // Display the lecturers in the view (you can use a dropdown or list)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lecturers.");
                return View("Error");  // Handle errors by showing an error view
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditLecturerDetails(int lecturerId)
        {
            try
            {
                var lecturer = ClaimDbContext.GetLecturerById(lecturerId);  // Get the lecturer by ID
                if (lecturer == null)
                {
                    return NotFound();  // If the lecturer is not found, return a Not Found response
                }
                return View(lecturer);  // Pass the lecturer to the view for editing
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving lecturer details.");
                return View("Error");  // Handle errors by showing an error view
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditLecturerDetails(Lecturer lecturer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = ClaimDbContext.SaveChanges(lecturer);  // Save changes to the lecturer
                    if (result)
                    {
                        return RedirectToAction("HRdash");  // Redirect to the index page or wherever necessary
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error occurred while saving the lecturer's details.");
                    }
                }
                return View(lecturer);  // Return the view with any errors for the user to correct
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating lecturer details.");
                return View("Error");  // Handle errors by showing an error view
            }
        }

        [HttpGet]
        public IActionResult GenerateReportForm()
        {
            return View();
        }

        [HttpPost]
       
        public async Task<IActionResult> GenerateReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                var approvedClaims = ClaimDbContext.GetApprovedClaimsByDateRange(startDate, endDate);

                if (approvedClaims == null || !approvedClaims.Any())
                {
                    TempData["Message"] = "No approved claims found in the specified date range.";
                    return RedirectToAction("GenerateReportForm");  // Redirect back to the form
                }

                decimal totalClaimsAmount = approvedClaims.Sum(c => c.HoursWorked * c.HourlyRate);

                var report = new Report
                {
                    ReportDate = DateTime.Now,
                    Claims = approvedClaims,
                    TotalClaimsAmount = totalClaimsAmount,
                    Summary = $"Report generated for approved claims between {startDate.ToShortDateString()} and {endDate.ToShortDateString()}."
                };

                return View("ReportView", report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating the report.");
                return View("Error");
            }
        }
    }
}
