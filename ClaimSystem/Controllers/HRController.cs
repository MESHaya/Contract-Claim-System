using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClaimSystem.Controllers
{
    public class HRController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly ILogger<HRController> _logger;

        private readonly ClaimDbContext _context;


       
        public HRController(ILogger<HRController> logger, ClaimDbContext context)
        {
            _logger = logger;
            _context = context;
            
        }

        public IActionResult ManageLecturers()
        {
            return View(); // Display a list of lecturers with edit options
        }

        public IActionResult GenerateReports()
        {
            return View(); // Display options for generating reports
        }

        //UPDATE LECTUERER DATA 
        public IActionResult EditLecturer(int id)
        {
            var lecturer = _context.Lecturers.FirstOrDefault(l => l.Id == id);
            return View(lecturer);
        }

        [HttpPost]
        public IActionResult EditLecturer(Lecturer model)
        {
            if (ModelState.IsValid)
            {
                _context.Lecturers.Update(model);
                _context.SaveChanges();
                return RedirectToAction("ManageLecturers");
            }
            return View(model);
        }

        public IActionResult GenerateClaimReports()
        {
            var approvedClaims = _context.Claim
                                         .Where(c => c.Status == "Approved")
                                         .ToList();

            // Generate a report (pseudo-code, replace with your reporting library logic)
            var report = new ClaimReportGenerator().Generate(approvedClaims);

            return File(report, "application/pdf", "ClaimReport.pdf");
        }


    }
}
