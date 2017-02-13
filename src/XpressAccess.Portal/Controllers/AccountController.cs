using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using XpressAccess.Portal.ViewModels;
using XpressAccess.Identity.Manager;
using XpressAccess.Identity.EFStore;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace XpressAccess.Portal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IdentityUserManager<IdentityUser> _userManager;

        public AccountController(IdentityUserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { Email = model.Email };
                if (await _userManager.CreateAsync(user, model.Password))
                {
                    // assign default role
                    await _userManager.AssignToRoleAsync(user, "ClientAdmin");
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError(string.Empty, "Register user failed");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/Login
        [HttpGet]
        public  IActionResult Login()
        {
            return RedirectToAction("Public", "Test");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Logout()
        {
            await HttpContext.Authentication.SignOutAsync("cookie");
            await HttpContext.Authentication.SignOutAsync("oidc");
        }
    }
}
