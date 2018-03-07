using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication0523mvc.Models;

namespace WebApplication0523mvc.Controllers
{
    [Authorize(Roles ="admin")]
    public class PayRollsController : Controller
    {
        private MovieDBContext db = new MovieDBContext();

        // GET: PayRolls
        public ActionResult Index()
        {
            return View(db.PayRolls.ToList());
        }

        // GET: PayRolls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayRoll payRoll = db.PayRolls.Find(id);
            if (payRoll == null)
            {
                return HttpNotFound();
            }
            return View(payRoll);
        }

        // GET: PayRolls/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PayRolls/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmployeeName,Salary,PayDate")] PayRoll payRoll)
        {
            if (ModelState.IsValid)
            {
                db.PayRolls.Add(payRoll);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payRoll);
        }

        // GET: PayRolls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayRoll payRoll = db.PayRolls.Find(id);
            if (payRoll == null)
            {
                return HttpNotFound();
            }
            return View(payRoll);
        }

        // POST: PayRolls/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeName,Salary,PayDate")] PayRoll payRoll)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payRoll).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payRoll);
        }

        // GET: PayRolls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayRoll payRoll = db.PayRolls.Find(id);
            if (payRoll == null)
            {
                return HttpNotFound();
            }
            return View(payRoll);
        }

        // POST: PayRolls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PayRoll payRoll = db.PayRolls.Find(id);
            db.PayRolls.Remove(payRoll);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
