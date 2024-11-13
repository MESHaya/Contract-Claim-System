using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClaimSystem.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ILogger<ManagerController> _logger;
        private readonly ClaimVerificationService _claimVerificationService;

        private readonly ClaimDbContext _context;
        public IActionResult Index()
        {
            return View();
        }

        public ManagerController(ILogger<ManagerController> logger, ClaimDbContext context, ClaimVerificationService claimVerificationService)
        {
            _logger = logger;
            _context = context;
            _claimVerificationService = claimVerificationService;
        }

        //View all pending claims for verification 

        public IActionResult Academic_Manager_Dash()
        {
            var pendingClaims = _context.Claim
                                        .Where(c => c.Status == "Pending")
                                        .ToList();

            // Only reject claims automatically if they don't follow the rules
            foreach (var claim in pendingClaims)
            {
                bool isValid = _claimVerificationService.VerifyClaim(claim);

                if (!isValid)
                {
                    // Set status to Rejected if the claim violates any rules
                    claim.Status = "Rejected";
                }
            }

            _context.SaveChanges(); 
            return View(pendingClaims);
        }




        [HttpPost]
        public IActionResult ApproveClaim(int claimId)
        {
            var claim = _context.Claim.FirstOrDefault(c => c.Id == claimId);
            if (claim != null && claim.Status == "Pending")
            {
                claim.Status = "Approved";
                _context.SaveChanges();
            }
            return RedirectToAction("Academic_Manager_Dash");
        }

        [HttpPost]
        public IActionResult RejectClaim(int claimId, string? rejectionReason)
        {
            var claim = _context.Claim.FirstOrDefault(c => c.Id == claimId);
            if (claim != null && claim.Status == "Pending")
            {
                claim.Status = "Rejected";
                claim.RejectionReason = rejectionReason;
                _context.SaveChanges();
            }
            return RedirectToAction("Academic_Manager_Dash");
        }


    }
}
