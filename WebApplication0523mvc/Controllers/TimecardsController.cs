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
    [Authorize]
    public class TimecardsController : Controller
    {
        private MovieDBContext db = new MovieDBContext();

        static int[] day = { 1, 2, 3, 4, 5, 6, 0 };
        private DateTime weekstart = DateTime.Now.AddDays(Convert.ToDouble((0 - day[Convert.ToInt16(DateTime.Now.DayOfWeek)])));
        private DateTime weekend = DateTime.Now.AddDays(Convert.ToDouble((6 - day[Convert.ToInt16(DateTime.Now.DayOfWeek)])));
        
        // GET: Timecards
        public ActionResult Index()
        {
            var timecards = from l in db.Timecards
                            select l;
            timecards = timecards.Where(s => s.EmployeeName.Equals(User.Identity.Name));
            timecards = timecards.Where(s => s.Date.Month == DateTime.Now.Month);//显示当月记录

            ViewBag.WeekStart = weekstart.ToString("yyyy-MM-dd");
            ViewBag.WeekEnd = weekend.ToString("yyyy-MM-dd");
            ViewBag.HourLeft = isOutOfLimit(0);

            return View(timecards);
        }

        // GET: Timecards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timecard timecard = db.Timecards.Find(id);
            if (timecard == null)
            {
                return HttpNotFound();
            }
            return View(timecard);
        }

        // GET: Timecards/Create
        public ActionResult Create()
        {
            var ChargeNumList = new List<string>();
            var ChargeNumQry = from d in db.ChargeNumbers
                               orderby d.ID
                               select d.chargenumber;
            ChargeNumList.AddRange(ChargeNumQry.Distinct());
            ViewBag.ChargeNumList = new SelectList(ChargeNumList);

            ViewBag.WeekStart = weekstart.ToString("yyyy-MM-dd");
            ViewBag.WeekEnd = weekend.ToString("yyyy-MM-dd");

            List<string> datelist = new List<string>();
            for(int i =0;i<7;i++)
                datelist.Add(weekstart.AddDays(i).ToString("yyyy-MM-dd"));
            ViewBag.DateList = new SelectList(datelist);


            return View();
        }

        // POST: Timecards/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Chargenumber,Hours,Date")] Timecard timecard)
        {
            ViewBag.WeekStart = weekstart.ToString("yyyy-MM-dd");
            ViewBag.WeekEnd = weekend.ToString("yyyy-MM-dd");

            var ChargeNumList = new List<string>();
            var ChargeNumQry = from d in db.ChargeNumbers
                               orderby d.ID
                               select d.chargenumber;
            ChargeNumList.AddRange(ChargeNumQry.Distinct());
            ViewBag.ChargeNumList = new SelectList(ChargeNumList);

            List<string> datelist = new List<string>();
            for (int i = 0; i < 7; i++)
                datelist.Add(weekstart.AddDays(i).ToString("yyyy-MM-dd"));
            ViewBag.DateList = new SelectList(datelist);

            timecard.EmployeeName = User.Identity.Name;
            timecard.StartDate = weekstart;
            timecard.EndDate = weekend;
            timecard.SubmitDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Timecards.Add(timecard);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(timecard);
        }

        // GET: Timecards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timecard timecard = db.Timecards.Find(id);
            if (timecard == null)
            {
                return HttpNotFound();
            }

            if (timecard.isSubmitted != null)
            {
                Response.Write("<script>alert('提交的记录不允许修改!')</script>");
                return RedirectToAction("Index");
            }
            //防止修改他人信息
            if(!timecard.EmployeeName.Equals(User.Identity.Name))
            {
                return RedirectToAction("Index");
            }

            var ChargeNumList = new List<string>();
            var ChargeNumQry = from d in db.ChargeNumbers
                               orderby d.ID
                               select d.chargenumber;
            ChargeNumList.AddRange(ChargeNumQry.Distinct());
            ViewBag.ChargeNumList = new SelectList(ChargeNumList);

            List<string> datelist = new List<string>();
            for (int i = 0; i < 7; i++)
                datelist.Add(weekstart.AddDays(i).ToString("yyyy-MM-dd"));
            ViewBag.DateList = new SelectList(datelist);

            return View(timecard);
        }

        // POST: Timecards/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Chargenumber,Hours,Date")] Timecard timecard)
        {
            var ChargeNumList = new List<string>();
            var ChargeNumQry = from d in db.ChargeNumbers
                               orderby d.ID
                               select d.chargenumber;
            ChargeNumList.AddRange(ChargeNumQry.Distinct());
            ViewBag.ChargeNumList = new SelectList(ChargeNumList);

            List<string> datelist = new List<string>();
            for (int i = 0; i < 7; i++)
                datelist.Add(weekstart.AddDays(i).ToString("yyyy-MM-dd"));
            ViewBag.DateList = new SelectList(datelist);

            timecard.EmployeeName = User.Identity.Name;
            var date = from d in db.Timecards
                       where d.ID == timecard.ID
                       select d;
            date.First().Chargenumber = timecard.Chargenumber;
            date.First().Hours = timecard.Hours;
            date.First().Date = timecard.Date;

            if (ModelState.IsValid)
            {
                db.Entry(date.First()).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timecard);
        }

        // GET: Timecards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timecard timecard = db.Timecards.Find(id);
            if (timecard == null)
            {
                return HttpNotFound();
            }
            if (timecard.isSubmitted != null)
            {
                Response.Write("<script>alert('提交的记录不允许修改!')</script>");
                return RedirectToAction("Index");
            }
            //防止修改他人信息
            if (!timecard.EmployeeName.Equals(User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            return View(timecard);
        }

        // POST: Timecards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Timecard timecard = db.Timecards.Find(id);
            db.Timecards.Remove(timecard);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Submit----GET
        public ActionResult Submit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timecard timecard = db.Timecards.Find(id);
            if (timecard == null)
            {
                return HttpNotFound();
            }
            if (timecard.isSubmitted != null)
            {
                Response.Write("<script>alert('不可重复提交!')</script>");
                return RedirectToAction("Index");
            }
            if (isOutOfLimit(timecard.Hours) < 0)
            {
                Response.Write("<script>alert('超出工作时间限制!')</script>");
                return RedirectToAction("Index");
            }
            timecard.isSubmitted = "已提交";
            if (ModelState.IsValid)
            {
                db.Entry(timecard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Response.Write("<script>alert('提交失败!')</script>");
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

        private int isOutOfLimit(int submithour)
        {
            //提取用户已提交的记录
            var mytimecards = from l in db.Timecards
                              where l.isSubmitted != null
                              select l;
            mytimecards = mytimecards.Where(s => s.EmployeeName.Equals(User.Identity.Name));

            var myinformation = from l in db.Employees
                                where l.Email == User.Identity.Name
                                select l;
            if (myinformation.Count() == 0)
                return -1;

            Employee me = myinformation.First();

            if(me.Position == position.HOUR)
            {
                //提取本周记录
                int totalsubmithour = 0;
                for (int i = 0; i < 7; i++)
                {
                    DateTime a = DateTime.Parse(weekstart.AddDays(i).ToString("yyyy-MM-dd"));
                    foreach (Timecard t in mytimecards)
                    {
                        if (t.Date.Equals(a))
                            totalsubmithour += t.Hours;
                    }
                }
                return me.HourLimit - (totalsubmithour + submithour);
            }
            else
            {
                int totalsubmithour = 0;
                foreach(Timecard t in mytimecards)
                {
                    if (t.Date.Month == DateTime.Now.Month)
                        totalsubmithour += t.Hours;
                }
                return me.HourLimit - (totalsubmithour + submithour);
            }
        }
    }
}
