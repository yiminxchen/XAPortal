using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace XpressAccess.Portal.Controllers
{
    /// <summary>
    /// Test Control to show authorization with Identity Server
    /// </summary>
    public class TestController : Controller
    {
        /// <summary>
        /// Public resource - Allow anonymous access
        /// </summary>
        [AllowAnonymous]
        public IActionResult Public()
        {
            ViewData["Title"] = "Anyone can access public data.";
            return View("Index");
        }

        /// <summary>
        /// Authorization resource - Allow PortalAdmin role access
        /// </summary>
        [Authorize("Portal Admin")]
        public IActionResult PortalAdmin()
        {
            ViewData["Title"] = "Only portal admin can access portal data.";
            SetEmailRole();
            return View("Index");
        }

        /// <summary>
        /// Authorization resource - Allow ClientAdmin role access
        /// </summary>
        [Authorize("Client Admin")]
        public IActionResult ClientAdmin()
        {
            ViewData["Title"] = "Either portal or client admin can access client data.";
            SetEmailRole();
            return View("Index");
        }

        /// <summary>
        /// Show forbidden access
        /// </summary>
        public IActionResult Forbidden()
        {
            ViewData["Title"] = "Authenticated, but you do not have privilege to access";
            SetEmailRole();
            return View("Index");
        }

        private void SetEmailRole()
        {
            ViewData["Email"] = User.Claims.SingleOrDefault(c => c.Type == "email").Value;
            ViewData["Role"] = User.Claims.FirstOrDefault(c => c.Type == "role").Value;
        }
    }
}
