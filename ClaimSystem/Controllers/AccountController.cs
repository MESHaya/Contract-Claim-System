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

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                // Username does not exist
                ModelState.AddModelError(string.Empty, "The username does not exist.");
                return View();
            }

            // Check the password
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Lecturer"))
                {
                    return RedirectToAction("LecturerDash", "Lecturer");
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
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your account has been locked out. Please try again later.");
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "You are not allowed to log in. Please check your email for confirmation.");
            }
            else
            {
                // Incorrect password or other failure
                ModelState.AddModelError(string.Empty, "The password is incorrect.");
            }

            return View();
        }

        // Handle logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
