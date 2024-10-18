using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpendSmart.Controllers;
using System.Threading.Tasks;

namespace ClaimSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly ILogger<AccountController> _logger;

        public AccountController(SignInManager<IdentityUser> signInManager, ILogger<AccountController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // Show login page
        public IActionResult Login()
        {
            return View();
        }

        // Handle login post request
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (result.Succeeded)
            {
                // Find the user by username
                var user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    // Get the user's roles
                    var roles = await _userManager.GetRolesAsync(user);
                    // Redirect based on the role
                    if (roles.Contains("Lecturer"))
                    {
                        return RedirectToAction("LecturerDash", "Lecturer"); // Redirect to Lecturer dashboard
                    }
                    else if (roles.Contains("Academic Manager"))
                    {
                        return RedirectToAction("Academic_Manager_Dash", "Manager"); 
                    }
                    else if (roles.Contains("Programme Coordinator"))
                    {
                        return RedirectToAction("Programme_Coordinator_Dash", "Programme"); 
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }

            // Fallback in case no role matched
            return RedirectToAction("Index", "Home");
        }

        // Handle logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
