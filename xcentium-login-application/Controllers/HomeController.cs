using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using xcentium_login_application.Models;

namespace xcentium_login_application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Login(UserModel user)
        //{
        //if (ModelState.IsValid)
        //{
        //    using (var reader = new StreamReader("./Assets/logindata.csv"))
        //    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        //    {
        //        IEnumerable<UserModel> userRecords = csv.GetRecords<UserModel>();
        //        if (userRecords.FirstOrDefault(userRecord => userRecord.Username == user.Username) != null)
        //        {
        //            Session["UserID"] = 
        //        }
        //    }
        //}
        //    return View();
        //}

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
