using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClaimSystem.Controllers
{
    public class ProgrammeController : Controller
    {
        private readonly ILogger<ProgrammeController> _logger;

        private readonly ClaimDbContext _context;
        public IActionResult Index()
        {
            return View();
        }

        public ProgrammeController(ILogger<ProgrammeController> logger, ClaimDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //View all pending claims for verification 

        public IActionResult Programme_Coordinator_Dash()
        {
            var pendingClaims = _context.Claim
                                .Where(c => c.Status == "Pending")
                                .ToList();
            return View(pendingClaims);
        }

        //Approve a claim 
        [HttpPost]
        public IActionResult ApproveClaim(int claimId)
        {
            var claim = _context.Claim.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                claim.Status = "Approved";
                _context.SaveChanges();
            }

            return RedirectToAction("Programme_Coordinator_Dash");
        }

        //Reject a claim 
        [HttpPost]
        public IActionResult RejectClaim(int claimId)
        {
            var claim = _context.Claim.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                claim.Status = "Rejected";
                _context.SaveChanges();
            }
            return RedirectToAction("Programme_Coordinator_Dash");
        }
    }
}
