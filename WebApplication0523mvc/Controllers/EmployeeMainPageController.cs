using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication0523mvc.Models;

namespace WebApplication0523mvc.Controllers
{
    [Authorize]
    public class EmployeeMainPageController : Controller
    {
        private MovieDBContext db = new MovieDBContext();

        // GET: EmployeeMainPage
        public ActionResult Index()
        {
            var myposition = from d in db.Employees
                             where d.Email == User.Identity.Name
                             select d.Position;

            if (myposition.First() == position.COMMISION)
                return RedirectToAction("Commission");
            return RedirectToAction("Salary");
        }

        public ActionResult Salary()
        {
            messageBox();
            return View();
        }

        public ActionResult Commission()
        {
            messageBox();
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void messageBox()
        {
            var mysalary = from s in db.PayRolls
                           orderby s.ID descending
                           where s.EmployeeName == User.Identity.Name
                           select s;
            ViewBag.LastPayDate = mysalary.First().PayDate.ToString();
        }
    }
}