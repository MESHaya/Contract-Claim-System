using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClaimSystem.Controllers
{
    public class HRController : Controller
    {
        private readonly ILogger<HRController> _logger;

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


        public IActionResult GenerateReport()
        {
            return View(); // Display options for generating reports
        }

        public IActionResult GenerateInvoice()
        {
            return View(); // Display options for generating invoices
        }

        public IActionResult GetAllLecturers()
        {
            List<Lecturer> lecturers = ClaimDbContext.GetAllLecturers(); // Fetch all lecturers
            return View(lecturers); // Pass the list to the view
        }


        // List all lecturers
        public IActionResult EditLecturer()
        {
            var lecturers = ClaimDbContext.GetAllLecturers();
            return View(lecturers); // Pass list of lecturers to the view
        }
        [HttpPost]
        public IActionResult EditAllLecturers(List<Lecturer> lecturers)
        {
            if (ModelState.IsValid)
            {
                foreach (var lecturer in lecturers)
                {
                    ClaimDbContext.UpdateLecturer(lecturer); // Update each lecturer
                }
                return RedirectToAction("HRdash"); // Redirect to a list or confirmation page
            }
            return View(lecturers); // Return the view with validation errors
        }




        //GET ALL CLAIMS
        public IActionResult ManageClaims()
        {
            var claimsList = ClaimDbContext.GetAllClaims();
            return View(claimsList);
        }

        //UPDATE CLAIM STATUS
        public IActionResult UpdateClaimStatus(int id, string status)
        {
            var claim = ClaimDbContext.GetAllClaims().FirstOrDefault(c => c.Id == id);
            if (claim != null)
            {
                claim.Status = status;
                bool success = ClaimDbContext.UpdateClaim(claim);
                if (success)
                {
                    // Redirect or return a success message
                    return RedirectToAction("ManageClaims");
                }
            }
            return View(); // Return an error view if update fails
        }

        // Example of generating a PDF report
        public IActionResult GenerateClaimReports()
        {
            var claims = ClaimDbContext.GetAllClaims();
            // You may implement the actual PDF generation logic here
            // For now, just returning a success response
            return File(new byte[0], "application/pdf", "ClaimReport.pdf");
        }
    }
}
