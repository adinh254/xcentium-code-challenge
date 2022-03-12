using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;

using CsvHelper;

using xcentium_code_challenge.Models;

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
            bool inSession = HttpContext.Session.GetString("UserId") != null && HttpContext.Session.GetString("Username") != null && HttpContext.Session.GetString("Password") != null;
            if (inSession)
            {
                ViewBag.Name = HttpContext.Session.GetString("Name");
                return View("Dashboard");
            }
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
                    UserModel foundUser = userRecords.FirstOrDefault(
                        userRecord => (userRecord.Username == user.Username) && (userRecord.Password == user.Password));
                    if (foundUser != null)
                    {
                        HttpContext.Session.SetString("UserId", foundUser.Id.ToString());
                        HttpContext.Session.SetString("Username", foundUser.Username.ToString());
                        HttpContext.Session.SetString("Password", foundUser.Password.ToString());
                        HttpContext.Session.SetString("Name", foundUser.Name.ToString());
                        return RedirectToAction("Dashboard");
                    }
                    ViewBag.Message = "Username or Password is incorrect.";
                    return View();
                }
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                ViewBag.Name = HttpContext.Session.GetString("Name");
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
