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

        [HttpPost]
        public async Task<IActionResult> CreateClaimForm(Claims model, List<IFormFile> files)
        {
            model.Status = "Pending";

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        // Generate a unique file name
                        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        var extension = Path.GetExtension(file.FileName);
                        var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                        // Path to save the file
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", uniqueFileName);

                        // Save file to the directory
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Store the file information in the database (assuming you have a field for FileName in Claims model)
                        model.FileName = uniqueFileName;
                    }
                }
            }

            _context.Claim.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("LecturerDash");
        }

        // GET: Track Claims (filtered by status)
        public IActionResult TrackClaims(string lecturerName)
        {
            var claims = _context.Claim
                .Where(c => c.ClaimName == lecturerName && c.Status != "Completed")
                .OrderBy(c => c.Status)
                .ToList();

            return View(claims);
        }


    }
}
