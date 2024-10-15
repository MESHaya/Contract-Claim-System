using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using ClaimSystem.Controllers;

namespace ClaimSystem.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ILogger<LecturerController > _logger;

        private readonly ClaimDbContext _context;

        public LecturerController(ILogger<LecturerController> logger, ClaimDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult LecturerDash()
        {
            return View();
        }


        public IActionResult Claim()
        {
            var allClaims = _context.Claim.ToList();
            return View(allClaims);
        }


        public IActionResult CreateClaim()
        {
            return View();
        }

        public IActionResult CreateClaimForm(Claims model)
        {

            model.Status = "Pending";
            _context.Claim.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Claim");
        }

        // GET: Track Claims (filtered by status)
        public IActionResult TrackClaims(string lecturerName)
        {
            var claims = _context.Claim
                .Where(c => c.LecturerName == lecturerName && c.Status != "Completed")
                .OrderBy(c => c.Status)
                .ToList();

            return View(claims);
        }


    }
}
