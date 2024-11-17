using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using ClaimSystem.Controllers;
using System.Collections.Generic;

namespace ClaimSystem.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ILogger<LecturerController > _logger;

        private static readonly string constr = "server=localhost;uid=root;pwd=Password1234567890;database=claimdb";


        private readonly ClaimVerificationService _claimVerificationService;
        public LecturerController(ILogger<LecturerController> logger,  ClaimVerificationService claimVerificationService)
        {
            _logger = logger;
        
            _claimVerificationService = claimVerificationService;
        }

        public IActionResult LecturerDash()
        {
            return View();
        }


        public IActionResult Claim()
        {
            var allClaims = ClaimDbContext.GetAllClaims(); 
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
            model.DateSubmitted = DateTime.Now; 

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

            bool claimCreated = ClaimDbContext.CreateClaim(model);

            if (claimCreated)
            {
                return RedirectToAction("LecturerDash");
            }
            else
            {
                ModelState.AddModelError("", "There was an error submitting the claim.");
                return View(model);
            }
        }



        // GET: Track Claims (filtered by status)
        public IActionResult TrackClaims(int userId)
        {
            try
            {
                // Fetch claims for a specific user from the database
                List<Claims> userClaims = ClaimDbContext.GetClaimsForUser(userId);

                if (userClaims == null || userClaims.Count == 0)
                {
                    ViewBag.Message = "No claims found for this user.";
                }

                return View(userClaims);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur
                Console.WriteLine("An error occurred while fetching claims: " + ex.Message);
                ViewBag.Message = "An error occurred while fetching claims. Please try again.";
                return View();
            }
        }


    }
}
