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
    public class PurchaseOrdersController : Controller
    {
        private MovieDBContext db = new MovieDBContext();

        // GET: PurchaseOrders
        public ActionResult Index(string searchID)
        {
            var orders = from l in db.PurchaseOrders
                            select l;
            orders = orders.Where(s => s.EmployeeName.Equals(User.Identity.Name));

            if (!string.IsNullOrEmpty(searchID))
            {
                if (searchID.Equals("all"))
                    return View(orders);

                try
                {
                    int id = int.Parse(searchID);
                    orders = orders.Where(s => s.ID.Equals(id));
                }
                catch (Exception e)
                {
                    Response.Write("<script>alert('请输入数字!')</script>");
                    return View(orders);
                }
                if (orders.Count() == 0)
                {
                    Response.Write("<script>alert('销售清单ID不存在!')</script>");
                    return View(orders);
                }
                return View(orders);
            }
            else
                return View(orders);
        }

        // GET: PurchaseOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PurchaseOrders/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Date,CustomerName,BillingAddress,Product,Total")] PurchaseOrder purchaseOrder)
        {
            purchaseOrder.EmployeeName = User.Identity.Name;

            if (ModelState.IsValid)
            {
                db.PurchaseOrders.Add(purchaseOrder);
                db.SaveChanges();
                return RedirectToAction("Details",new { id = purchaseOrder.ID});
            }

            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            if (purchaseOrder.isClosed != null)
            {
                Response.Write("<script>alert('交易已关闭!')</script>");
                return RedirectToAction("Index");
            }
            //防止修改他人信息
            if (!purchaseOrder.EmployeeName.Equals(User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Date,CustomerName,BillingAddress,Product,Total")] PurchaseOrder purchaseOrder)
        {
            purchaseOrder.EmployeeName = User.Identity.Name;

            if (ModelState.IsValid)
            {
                db.Entry(purchaseOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            //防止修改他人信息
            if (!purchaseOrder.EmployeeName.Equals(User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            db.PurchaseOrders.Remove(purchaseOrder);
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
