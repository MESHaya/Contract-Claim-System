using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ClaimSystem.Controllers
{
    public class ProgrammeController : Controller
    {
        private readonly ILogger<ProgrammeController> _logger;
        private readonly ClaimVerificationService _claimVerificationService;

        public ProgrammeController(ILogger<ProgrammeController> logger, ClaimVerificationService claimVerificationService)
        {
            _logger = logger;
            _claimVerificationService = claimVerificationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // View all pending claims for verification
        public IActionResult Programme_Coordinator_Dash()
        {
            var pendingClaims = ClaimDbContext.GetAllClaims()
                                                .Where(c => c.Status == "Pending")
                                                .ToList();

            // Automatically reject claims if they don't meet verification rules
            foreach (var claim in pendingClaims)
            {
                var validationResult = _claimVerificationService.VerifyClaim(claim);

                if (!validationResult.IsValid)
                {
                    // Update status to Rejected and store the rejection reason(s)
                    claim.Status = "Rejected";
                    claim.RejectionReason = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                    // Persist changes to MySQL
                    ClaimDbContext.UpdateClaim(claim); // You'll need a method like this in ClaimDbContext
                }
            }

            return View(pendingClaims);
        }

        // Manually approve a pending claim
        [HttpPost]
        public IActionResult ApproveClaim(int claimId)
        {
            var claim = ClaimDbContext.GetAllClaims().FirstOrDefault(c => c.Id == claimId);
            if (claim != null && claim.Status == "Pending")
            {
                claim.Status = "Approved";
                ClaimDbContext.UpdateClaim(claim); // Persist approval
            }
            return RedirectToAction(" Programme_Coordinator_Dash");
        }

        // Manually reject a pending claim with a reason
        [HttpPost]
        public IActionResult RejectClaim(int claimId, string? rejectionReason)
        {
            var claim = ClaimDbContext.GetAllClaims().FirstOrDefault(c => c.Id == claimId);
            if (claim != null && claim.Status == "Pending")
            {
                claim.Status = "Rejected";
                claim.RejectionReason = rejectionReason ?? "No specific reason provided.";
                ClaimDbContext.UpdateClaim(claim); // Persist rejection
            }
            return RedirectToAction(" Programme_Coordinator_Dash");
        }
    }
}
