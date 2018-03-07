using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication0523mvc.Models;

namespace WebApplication0523mvc.Controllers
{
    public class HomeController : Controller
    {
        private MovieDBContext db = new MovieDBContext();

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return View();
            if (User.Identity.Name.Equals("Admin@admin.com"))
                return RedirectToAction("Index","Admin");

            return RedirectToAction("Index","EmployeeMainPage");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}