using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using xcentium_code_challenge.Models;
using CsvHelper;

namespace xcentium_code_challenge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserModel user)
        {
            if (ModelState.IsValid)
            {
                using (var reader = new StreamReader("./Data/logindata.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    IEnumerable<UserModel> userRecords = csv.GetRecords<UserModel>();
                    UserModel foundUser = userRecords.FirstOrDefault(userRecord => userRecord.Username == user.Username);
                    if (foundUser != null)
                    {
                        HttpContext.Session.SetString("UserId", foundUser.Id.ToString());
                        HttpContext.Session.SetString("Username", foundUser.Username.ToString());
                        return RedirectToAction("Dashboard");
                    }
                }
            }
            return View(user);
        }

        public IActionResult Dashboard(string username)
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                return View();
            }
            return RedirectToAction("Login");
        }

        public IActionResult Index()
        {
            return View();
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
