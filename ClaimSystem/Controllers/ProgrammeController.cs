using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation.Results;

namespace ClaimSystem.Controllers
{
    public class ProgrammeController : Controller
    {
        private readonly ILogger<ProgrammeController> _logger;
        private readonly ClaimVerificationService _claimVerificationService;
        private readonly ClaimDbContext _context;

        public ProgrammeController(ILogger<ProgrammeController> logger, ClaimDbContext context, ClaimVerificationService claimVerificationService)
        {
            _logger = logger;
            _context = context;
            _claimVerificationService = claimVerificationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // View all pending claims for verification
        public IActionResult Programme_Coordinator_Dash()
        {
            var pendingClaims = _context.Claim
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
                }
            }

            // Save all changes to the database at once
            _context.SaveChanges();
            return View(pendingClaims);
        }

        // Manually approve a pending claim
        [HttpPost]
        public IActionResult ApproveClaim(int claimId)
        {
            var claim = _context.Claim.FirstOrDefault(c => c.Id == claimId);
            if (claim != null && claim.Status == "Pending")
            {
                claim.Status = "Approved";
                _context.SaveChanges();
            }
            return RedirectToAction("Programme_Coordinator_Dash");
        }

        // Manually reject a pending claim with a reason
        [HttpPost]
        public IActionResult RejectClaim(int claimId, string? rejectionReason)
        {
            var claim = _context.Claim.FirstOrDefault(c => c.Id == claimId);
            if (claim != null && claim.Status == "Pending")
            {
                claim.Status = "Rejected";
                claim.RejectionReason = rejectionReason ?? "No specific reason provided.";
                _context.SaveChanges();
            }
            return RedirectToAction("Programme_Coordinator_Dash");
        }
    }
}
