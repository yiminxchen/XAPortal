using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace XpressAccess.WebData.Service.Controllers
{
    public class WebDataController : Controller
    {
        // public data, no need to authorize
        [HttpGet("/public")]
        public string Get()
        {
            return "Anyone can access public data.";
        }

        [HttpGet("/portal")]
        [Authorize("Portal Admin")]
        public string PortalAdmin()
        {
            return "Only portal admin can access portal data.";
        }

        [HttpGet("/client")]
        [Authorize("Client Admin")]
        public string ClientAdmin()
        {
            return "Either portal or client admin can access client data.";
        }

    }
}
