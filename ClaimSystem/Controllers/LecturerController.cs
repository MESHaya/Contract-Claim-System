using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using ClaimSystem.Controllers;

namespace ClaimSystem.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ILogger<LecturerController > _logger;

        private readonly ClaimDbContext _context;

        private readonly ClaimVerificationService _claimVerificationService;
        public LecturerController(ILogger<LecturerController> logger, ClaimDbContext context, ClaimVerificationService claimVerificationService)
        {
            _logger = logger;
            _context = context;
            _claimVerificationService = claimVerificationService;
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

        [HttpPost]
    
        public async Task<IActionResult> CreateClaimForm(Claims model, List<IFormFile> files)
        {
            //check if numbers are positive 

            if(model.HoursWorked <0)
            {
                ModelState.AddModelError("HoursWorked", "Cannot be negative"); 
            } 
            if(model.HourlyRate < 0)
            {
                ModelState.AddModelError("HourlyRate", "Cannot be negative");
            }
            if(ModelState.IsValid)
            {
                return View(model);
            }

            model.Status = "Pending";

            if (files != null && files.Count > 0)
            {
                List<string> fileNames = new List<string>();

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Save the file name or path to the list
                        fileNames.Add(file.FileName);
                    }
                }

                // Store the file names (or full paths) in the model
                model.FileName = string.Join(",", fileNames);

            }

           

            _context.Claim.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("LecturerDash");
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
