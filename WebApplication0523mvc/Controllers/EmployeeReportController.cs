using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication0523mvc.Models;

namespace WebApplication0523mvc.Controllers
{
    [Authorize]
    public class EmployeeReportController : Controller
    {
        private static EmployeeReport EMPLOYEEREPORT = new EmployeeReport();
        private MovieDBContext db = new MovieDBContext();

        // GET: employeereport
        public ActionResult Index()
        {
            var ChargeNumList = new List<string>();
            var ChargeNumQry = from d in db.ChargeNumbers
                               orderby d.ID
                               select d.chargenumber;
            ChargeNumList.AddRange(ChargeNumQry.Distinct());
            ViewBag.ChargeNumList = new SelectList(ChargeNumList);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "ReportType,StartDate,EndDate,Chargenumber")] EmployeeReport employeereport)
        {
            var ChargeNumList = new List<string>();
            var ChargeNumQry = from d in db.ChargeNumbers
                               orderby d.ID
                               select d.chargenumber;
            ChargeNumList.AddRange(ChargeNumQry.Distinct());
            ViewBag.ChargeNumList = new SelectList(ChargeNumList);

            if (employeereport.ReportType == EMreporttype.TOTALHOUR)//累计工时
            {
                employeereport.TotalHour = countHour(employeereport);
                employeereport.Name = getName();
                EMPLOYEEREPORT = employeereport;
                return RedirectToAction("HourReport");
            }
            else if(employeereport.ReportType == EMreporttype.YEARtoDATE)//累计薪水
            {
                employeereport.Salary = countSalary(employeereport);
                employeereport.Year = employeereport.EndDate.Year;
                employeereport.Name = getName();
                EMPLOYEEREPORT = employeereport;
                return RedirectToAction("SalaryReport");
            }
            else if(employeereport.ReportType == EMreporttype.HOURSforPROJECT)//项目累计时间
            {
                if(employeereport.Chargenumber == null)
                {
                    Response.Write("<script>alert('请选择项目编号!')</script>");
                    return View(employeereport);
                }
                employeereport.TotalHour = countHoursForProject(employeereport);
                employeereport.Name = getName();
                EMPLOYEEREPORT = employeereport;
                return RedirectToAction("HoursForProject"); ;
            }
            else//剩余假期
            {
                employeereport.Vacation = countVacation();
                employeereport.Name = getName();
                EMPLOYEEREPORT = employeereport;
                return RedirectToAction("Vacation"); ;
            }
        }

        public ActionResult HourReport()
        {
            return View(EMPLOYEEREPORT);
        }

        public ActionResult SalaryReport()
        {
            return View(EMPLOYEEREPORT);
        }

        public ActionResult HoursForProject()
        {
            return View(EMPLOYEEREPORT);
        }

        public ActionResult Vacation()
        {
            return View(EMPLOYEEREPORT);
        }

        public ActionResult generateReport()
        {
            Print();
            return View(EMPLOYEEREPORT);
        }

        public void Print()
        {
            //命名导出表格的StringBuilder变量
            StringBuilder sHtml = new StringBuilder(string.Empty);

            //打印表头
            sHtml.Append("<table border=\"1\" width=\"100%\">");
            if (EMPLOYEEREPORT.ReportType == EMreporttype.TOTALHOUR)
            {
                sHtml.Append("<tr height=\"40\"><td colspan=\"4\" align=\"center\" style='font-size:24px'><b>员工累计工时报告" + "</b></td></tr>");
                sHtml.Append("<tr height=\"20\" align=\"center\"><td>起始日期</td><td>结束日期</td><td>员工姓名</td><td>累计工时</td></tr>");
                sHtml.Append("<tr height=\"20\" align=\"left\"><td>" + EMPLOYEEREPORT.StartDate + "</td><td>" + EMPLOYEEREPORT.EndDate + "</td><td>" + EMPLOYEEREPORT.Name + "</td><td>" + EMPLOYEEREPORT.TotalHour + "</td></tr>");
                sHtml.Append("</table>");
                ExportToExcel("application/ms-excel", EMPLOYEEREPORT.Name+"累计工时报告.xls", sHtml.ToString());
            }
            else if(EMPLOYEEREPORT.ReportType == EMreporttype.HOURSforPROJECT)
            {
                sHtml.Append("<tr height=\"40\"><td colspan=\"5\" align=\"center\" style='font-size:24px'><b>员工年度累计薪水报告" + "</b></td></tr>");
                sHtml.Append("<tr height=\"20\" align=\"center\"><td>起始日期</td><td>结束日期</td><td>员工姓名</td><td>项目编号</td><td>累计工时</td></tr>");
                sHtml.Append("<tr height=\"20\" align=\"left\"><td>" + EMPLOYEEREPORT.StartDate + "</td><td>" + EMPLOYEEREPORT.EndDate + "</td><td>" + EMPLOYEEREPORT.Name + "</td><td>" + EMPLOYEEREPORT.Chargenumber + "</td><td>"+EMPLOYEEREPORT.TotalHour+"</td></tr>");
                sHtml.Append("</table>");
                ExportToExcel("application/ms-excel", EMPLOYEEREPORT.Name+"项目累计工时报告.xls", sHtml.ToString());
            }
            else if(EMPLOYEEREPORT.ReportType == EMreporttype.VACATION)
            {
                sHtml.Append("<tr height=\"40\"><td colspan=\"4\" align=\"center\" style='font-size:24px'><b>剩余年假报告" + "</b></td></tr>");
                sHtml.Append("<tr height=\"20\" align=\"center\"><td>起始日期</td><td>结束日期</td><td>员工姓名</td><td>剩余年假</td></tr>");
                sHtml.Append("<tr height=\"20\" align=\"left\"><td>" + EMPLOYEEREPORT.StartDate + "</td><td>" + EMPLOYEEREPORT.EndDate + "</td><td>" + EMPLOYEEREPORT.Name + "</td><td>" + EMPLOYEEREPORT.Vacation + "</td></tr>");
                sHtml.Append("</table>");
                ExportToExcel("application/ms-excel", EMPLOYEEREPORT.Name + "剩余年假报告.xls", sHtml.ToString());
            }
            else
            {
                sHtml.Append("<tr height=\"40\"><td colspan=\"4\" align=\"center\" style='font-size:24px'><b>员工年度累计薪水报告" + "</b></td></tr>");
                sHtml.Append("<tr height=\"20\" align=\"center\"><td>年份</td><td>结束日期</td><td>员工姓名</td><td>年度累计收入</td></tr>");
                sHtml.Append("<tr height=\"20\" align=\"left\"><td>" + EMPLOYEEREPORT.Year + "</td><td>" + EMPLOYEEREPORT.EndDate + "</td><td>" + EMPLOYEEREPORT.Name + "</td><td>" + EMPLOYEEREPORT.Salary + "</td></tr>");
                sHtml.Append("</table>");
                ExportToExcel("application/ms-excel", EMPLOYEEREPORT.Name + "年度累计薪水报告.xls", sHtml.ToString());
            }


        }

        //输入HTTP头，然后把指定的流输出到指定的文件名，然后指定文件类型
        public void ExportToExcel(string FileType, string FileName, string ExcelContent)
        {
            System.Web.HttpContext.Current.Response.ContentType = FileType;
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8).ToString());

            System.IO.StringWriter tw = new System.IO.StringWriter();
            //System.Web.HttpContext.Current.Response.Output.Write(ExcelContent.ToString());
            /*乱码BUG修改 20140505*/
            System.Web.HttpContext.Current.Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=utf-8\"/>" + ExcelContent.ToString());
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
        }

        //计算累计工时
        private int countHour(EmployeeReport employeereport)
        {
            var timecards = from t in db.Timecards
                            where t.EmployeeName == User.Identity.Name && t.isSubmitted != null
                            select t;
            int totalhour = 0;
            foreach (Timecard t in timecards)
            {
                if (DateTime.Compare(t.Date, employeereport.StartDate) >= 0 && DateTime.Compare(t.Date, employeereport.EndDate) <= 0)
                    totalhour += t.Hours;
            }
            return totalhour;
        }

        //计算年度累计薪水
        private decimal countSalary(EmployeeReport employeereport)
        {
            var payrolls = from p in db.PayRolls
                           where p.EmployeeName == User.Identity.Name
                           select p;
            decimal totalsalary = 0;
            foreach (PayRoll p in payrolls)
            {
                if (p.PayDate.Year == employeereport.EndDate.Year && DateTime.Compare(p.PayDate, employeereport.EndDate) <= 0)
                    totalsalary += p.Salary;
            }
            return totalsalary;
        }

        //计算项目时间
        private int countHoursForProject(EmployeeReport employeereport)
        {
            var timecards = from t in db.Timecards
                            where t.EmployeeName == User.Identity.Name && t.isSubmitted != null
                            select t;
            timecards = timecards.Where(t => t.Chargenumber.Equals(employeereport.Chargenumber));
            int totalhour = 0;
            foreach (Timecard t in timecards)
            {
                if (DateTime.Compare(t.Date, employeereport.StartDate) >= 0 && DateTime.Compare(t.Date, employeereport.EndDate) <= 0)
                    totalhour += t.Hours;
            }
            return totalhour;
        }

        //计算假期
        private int countVacation()
        {
            var myvacation = from m in db.Employees
                             where m.Email == User.Identity.Name
                             select m;
            return myvacation.First().Vacation;
        }

        //通过用户邮箱获取姓名
        private string getName()
        {
            var myname = from m in db.Employees
                         where m.Email == User.Identity.Name
                         select m;
            return myname.First().Name;
        }
    }
}