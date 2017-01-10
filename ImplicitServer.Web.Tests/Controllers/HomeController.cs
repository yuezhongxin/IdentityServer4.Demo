using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ImplicitServerWithJS.Web.Tests.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secure()
        {
            ViewData["Message"] = "Secure page.";

            return View();
        }

        public async Task Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            await HttpContext.Authentication.SignOutAsync("oidc");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}