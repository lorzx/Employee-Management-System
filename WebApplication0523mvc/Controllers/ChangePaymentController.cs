using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication0523mvc.Models;

namespace WebApplication0523mvc.Controllers
{
    [Authorize]
    public class ChangePaymentController : Controller
    {
        private MovieDBContext db = new MovieDBContext();

        // GET: ChangePayment
        public ActionResult Index()
        {
            var myinformation = from m in db.Employees
                                where m.Email == User.Identity.Name
                                select m;
            ViewBag.PaymentMethod = myinformation.First().PaymentMethod.ToString();
            return View();
        }

        public ActionResult PickUp()
        {
            var myinformation = from m in db.Employees
                                where m.Email == User.Identity.Name
                                select m;
            myinformation.First().PaymentMethod = paymentmethod.PICKUP;
            if (ModelState.IsValid)
            {
                db.Entry(myinformation.First()).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return Content("修改失败");
        }

        //MAIL----GET
        public ActionResult Mail()
        {
            return View();
        }

        //MAIL----POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Mail([Bind(Include = "MailAddress")] Payment payment)
        {
            if(payment.MailAddress == null)
            {
                Response.Write("<script>alert('请填写所有信息!')</script>");
                return View(payment);
            }
            var myinformation = from m in db.Employees
                                where m.Email == User.Identity.Name
                                select m;
            myinformation.First().PaymentMethod = paymentmethod.MAIL;
            myinformation.First().MailAddress = payment.MailAddress;
            if (ModelState.IsValid)
            {
                db.Entry(myinformation.First()).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payment);
        }

        //MAIL----GET
        public ActionResult Bank()
        {
            return View();
        }

        //MAIL----POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bank([Bind(Include = "BankName,BankAccount")] Payment payment)
        {
            if (payment.BankName == null || payment.BankAccount == null)
            {
                Response.Write("<script>alert('请填写所有信息!')</script>");
                return View(payment);
            }
            var myinformation = from m in db.Employees
                                where m.Email == User.Identity.Name
                                select m;
            myinformation.First().PaymentMethod = paymentmethod.TRANSFER;
            myinformation.First().BankName = payment.BankName;
            myinformation.First().BankAccount = payment.BankAccount;
            if (ModelState.IsValid)
            {
                db.Entry(myinformation.First()).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payment);
        }
    }
}