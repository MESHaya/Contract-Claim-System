using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using ClaimSystem.Models;
using System.Diagnostics;

namespace SpendSmart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ClaimDbContext _context;

        public HomeController(ILogger<HomeController> logger, ClaimDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
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




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
