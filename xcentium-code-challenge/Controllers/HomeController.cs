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

        // Initial action if it's a user's first time logging in or if they aren't in a session.
        // Redirects to the Dashboard View if the user is in a session. Otherwise returns a Login view.
        public IActionResult Login()
        {
            bool inSession = HttpContext.Session.GetString("UserId") != null && HttpContext.Session.GetString("Username") != null && HttpContext.Session.GetString("Password") != null;
            if (inSession)
            {
                // Redirects the user to their dashboard
                ViewBag.Name = HttpContext.Session.GetString("Name");
                return View("Dashboard");
            }
            return View();
        }

        // Overloaded action from the Login view whenever a user makes a form request.
        // Validation checks against the csv database will be executed here.
        // Will redirect to the Dashboard view if credentials are valid.
        // Otherwise returns back the Login view with error messages.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserModel user)
        {
            if (ModelState.IsValid)
            {
                // Gets all the rows through the Csv parser and iterates through all of the mapped rows and checks them against the submitted user model.
                using (var reader = new StreamReader("./Data/logindata.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    IEnumerable<UserModel> userRecords = csv.GetRecords<UserModel>();
                    UserModel foundUser = userRecords.FirstOrDefault(
                        userRecord => (userRecord.Username == user.Username) && (userRecord.Password == user.Password));
                    if (foundUser != null)
                    {
                        // Sets the session to the submitted user.
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
            // Reaches here only if the user doesn't submit a user with a username or password.
            return View(user);
        }

        // Action executed when the Logout button is clicked for the user.
        // Simply clears the current browser session and redirects back to the default Login view.
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // View that the user will see when they successfully log in.
        // Dashboard can only be reached if a session with a UserId exists.
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                // User's name gets passed into the viewbag for simple display in the  view.
                ViewBag.Name = HttpContext.Session.GetString("Name");
                return View();
            }
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
