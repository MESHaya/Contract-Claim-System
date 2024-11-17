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

        public IActionResult ManageLecturers()
        {
            return View(); // Display a list of lecturers with edit options
        }

        public IActionResult GenerateReport()
        {
            return View(); // Display options for generating reports
        }

        public IActionResult GenerateInvoice()
        {
            return View(); // Display options for generating invoices
        }

        //UPDATE LECTURER DATA 
        public IActionResult EditLecturer(int id)
        {
            var lecturer = ClaimDbContext.GetLecturerDetails(id);
            return View(lecturer);
        }

        [HttpPost]
        public IActionResult EditLecturer(Lecturer model)
        {
            if (ModelState.IsValid)
            {
                // Update Lecturer details in the database, assuming method available
                // You may need to implement a static update method in ClaimDbContext if necessary
                // For now, it just fetches data
                return RedirectToAction("ManageLecturers");
            }
            return View(model);
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
