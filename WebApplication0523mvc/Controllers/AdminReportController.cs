using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication0523mvc.Models;

namespace WebApplication0523mvc.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminReportController : Controller
    {
        private static AdminReport ADMINREPORT = new AdminReport();
        private MovieDBContext db = new MovieDBContext();

        // GET: AdminReport
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "ReportType,StartDate,EndDate,EmployeeName")] AdminReport adminreport)
        {
            if (adminreport.ReportType == reporttype.TOTALHOUR)
            {
                int hours = countHour(adminreport);
                if(hours == -1)
                {
                    Response.Write("<script>alert('找不到该员工!')</script>");
                    return View(adminreport);
                }
                adminreport.TotalHour = hours;
                ADMINREPORT = adminreport;
                return RedirectToAction("HourReport");
            }
            else
            {
                decimal salary = countSalary(adminreport);
                if (salary == -1)
                {
                    Response.Write("<script>alert('找不到该员工!')</script>");
                    return View(adminreport);
                }
                adminreport.Salary = salary;
                adminreport.Year = adminreport.EndDate.Year;
                ADMINREPORT = adminreport;
                return RedirectToAction("SalaryReport");
            }
        }

        public ActionResult HourReport()
        {
            return View(ADMINREPORT);
        }

        public ActionResult SalaryReport()
        {
            return View(ADMINREPORT);
        }

        public ActionResult generateReport()
        {
            Print();
            return View(ADMINREPORT);
        }

        public void Print()
        {
            //命名导出表格的StringBuilder变量
            StringBuilder sHtml = new StringBuilder(string.Empty);

            //打印表头
            sHtml.Append("<table border=\"1\" width=\"100%\">");

            if (ADMINREPORT.ReportType == reporttype.TOTALHOUR)
            {
                sHtml.Append("<tr height=\"40\"><td colspan=\"4\" align=\"center\" style='font-size:24px'><b>员工累计工时报告" + "</b></td></tr>");
                sHtml.Append("<tr height=\"20\" align=\"center\"><td>起始日期</td><td>结束日期</td><td>员工姓名</td><td>累计工时</td></tr>");
                sHtml.Append("<tr height=\"20\" align=\"left\"><td>" + ADMINREPORT.StartDate + "</td><td>" + ADMINREPORT.EndDate + "</td><td>" + ADMINREPORT.EmployeeName + "</td><td>" + ADMINREPORT.TotalHour + "</td></tr>");
            }
            else
            {
                sHtml.Append("<tr height=\"40\"><td colspan=\"4\" align=\"center\" style='font-size:24px'><b>员工年度累计薪水报告" + "</b></td></tr>");
                sHtml.Append("<tr height=\"20\" align=\"center\"><td>年份</td><td>结束日期</td><td>员工姓名</td><td>年度累计收入</td></tr>");
                sHtml.Append("<tr height=\"20\" align=\"left\"><td>" + ADMINREPORT.Year + "</td><td>" + ADMINREPORT.EndDate + "</td><td>" + ADMINREPORT.EmployeeName + "</td><td>" + ADMINREPORT.Salary + "</td></tr>");
            }
            //打印表尾
            sHtml.Append("</table>");

            //调用输出Excel表的方法
            if(ADMINREPORT.ReportType == reporttype.TOTALHOUR)
                ExportToExcel("application/ms-excel", ADMINREPORT.EmployeeName + "累计工时报告.xls", sHtml.ToString());
            else
                ExportToExcel("application/ms-excel", ADMINREPORT.EmployeeName + "年度累计薪水报告.xls", sHtml.ToString());
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
        private int countHour(AdminReport adminreport)
        {
            var myemail = from m in db.Employees
                          where m.Name == adminreport.EmployeeName
                          select m;
            if (myemail.Count() == 0)//找不到员工姓名，返回-1
            {
                return -1;
            }
            string temp = myemail.First().Email;
            var timecards = from t in db.Timecards
                            where t.EmployeeName == temp && t.isSubmitted != null
                            select t;
            int totalhour = 0;
            foreach (Timecard t in timecards)
            {
                if (DateTime.Compare(t.Date, adminreport.StartDate) >= 0 && DateTime.Compare(t.Date, adminreport.EndDate) <= 0)
                    totalhour += t.Hours;
            }
            return totalhour;
        }

        //计算年度累计薪水
        private decimal countSalary(AdminReport adminreport)
        {
            var myemail = from m in db.Employees
                          where m.Name == adminreport.EmployeeName
                          select m;
            if (myemail.Count() == 0)//找不到员工姓名，返回-1
            {
                return -1;
            }
            string temp = myemail.First().Email;
            var payrolls = from p in db.PayRolls
                           where p.EmployeeName == temp
                           select p;
            decimal totalsalary = 0;
            foreach(PayRoll p in payrolls)
            {
                if (p.PayDate.Year == adminreport.EndDate.Year && DateTime.Compare(p.PayDate, adminreport.EndDate) <= 0)
                    totalsalary += p.Salary;
            }
            return totalsalary;
        }
    }
}