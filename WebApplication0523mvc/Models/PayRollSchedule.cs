using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication0523mvc.Models
{
    public class PayRollSchedule : Registry
    {
        public PayRollSchedule()
        {
            Schedule<PayHour>().ToRunEvery(2).Minutes();
            Schedule<PaySalary>().ToRunEvery(2).Minutes();
            Schedule<PayCommission>().ToRunEvery(2).Minutes();

            //Schedule<PayHour>().ToRunEvery(1).Weeks().On(DayOfWeek.Friday).At(22, 0);
            //Schedule<PaySalary>().ToRunEvery(1).Months().On(getLastWorkDay()).At(22,0);
            //Schedule<PayCommission>().ToRunEvery(1).Months().On(getLastWorkDay()).At(22,0);
        }

        //获取每月最后一个工作日
        private int getLastWorkDay()
        {
            DateTime t1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
            if (t1.DayOfWeek == DayOfWeek.Saturday)
                return t1.Day - 1;
            else if (t1.DayOfWeek == DayOfWeek.Sunday)
                return t1.Day - 2;
            else
                return t1.Day;
        }
    }

    //时工薪水发放方法
    public class PayHour : IJob
    {
        private MovieDBContext db = new MovieDBContext();

        static int[] day = { 1, 2, 3, 4, 5, 6, 0 };
        private DateTime weekstart = DateTime.Now.AddDays(Convert.ToDouble((0 - day[Convert.ToInt16(DateTime.Now.DayOfWeek)])));
        private DateTime weekend = DateTime.Now.AddDays(Convert.ToDouble((6 - day[Convert.ToInt16(DateTime.Now.DayOfWeek)])));

        void IJob.Execute()
        {
            Trace.WriteLine("Hour");
            var houremployees = from h in db.Employees//找出所有时工
                                where h.Position == position.HOUR
                                select h;

            List<string> datelist = new List<string>();
            for (int i = 0; i < 7; i++)
                datelist.Add(weekstart.AddDays(i).ToString("yyyy-MM-dd"));

            foreach (Employee e in houremployees.ToList())
            {
                decimal totalhour = 0;

                var timecards = from h in db.Timecards
                                where h.EmployeeName == e.Email
                                select h;
                timecards = timecards.Where(t => t.isSubmitted != null);

                foreach (Timecard t in timecards)
                {
                    foreach (string s in datelist)
                    {
                        if (t.Date.ToString("yyyy-MM-dd").Equals(s))
                        {
                            if (t.Hours <= 8)
                                totalhour += t.Hours;
                            else
                                totalhour += t.Hours * 1.5M;
                        }
                    }
                }

                Trace.WriteLine(e.Email + ":" + (totalhour * e.HourRate - e.Deduction));
                ///*
                PayRoll p = new PayRoll();
                p.EmployeeName = e.Email;
                p.Salary = totalhour * e.HourRate - e.Deduction;
                p.PayDate = DateTime.Now;
                p.PaymentMethod = e.PaymentMethod;
                if (e.PaymentMethod == paymentmethod.MAIL)
                    p.MailAddress = e.MailAddress;
                if(e.PaymentMethod == paymentmethod.TRANSFER)
                {
                    p.BankName = e.BankName;
                    p.BankAccount = e.BankAccount;
                }
                db.PayRolls.Add(p);
                db.SaveChanges();
                //*/
                //如果员工标记为要删除，则付完薪水后删除
                if (e.isDeleted != null)
                {
                    db.Employees.Remove(e);
                    db.SaveChanges();
                }
            }
        }
    }

    //普通员工薪水发放方法
    public class PaySalary : IJob
    {
        private MovieDBContext db = new MovieDBContext();

        void IJob.Execute()
        {
            Trace.WriteLine("Salary");
            var salaryemployees = from h in db.Employees//找出所有普通员工
                                  where h.Position == position.SALARY
                                  select h;

            foreach (Employee e in salaryemployees.ToList())
            {
                Trace.WriteLine(e.Email + ":" + (e.Salary - e.Deduction));
                ///*
                PayRoll p = new PayRoll();
                p.EmployeeName = e.Email;
                p.Salary = e.Salary - e.Deduction;
                p.PayDate = DateTime.Now;
                p.PaymentMethod = e.PaymentMethod;
                if (e.PaymentMethod == paymentmethod.MAIL)
                    p.MailAddress = e.MailAddress;
                if (e.PaymentMethod == paymentmethod.TRANSFER)
                {
                    p.BankName = e.BankName;
                    p.BankAccount = e.BankAccount;
                }
                db.PayRolls.Add(p);
                db.SaveChanges();
                //*/
                //如果员工标记为要删除，则付完薪水后删除
                if (e.isDeleted != null)
                {
                    db.Employees.Remove(e);
                    db.SaveChanges();
                }
            }
        }
    }

    //业绩提成员工薪水发放方法
    public class PayCommission : IJob
    {
        private MovieDBContext db = new MovieDBContext();

        void IJob.Execute()
        {
            Trace.WriteLine("Commission");
            var commissionemployees = from h in db.Employees//找出所有提成员工
                                      where h.Position == position.COMMISION
                                      select h;

            foreach (Employee e in commissionemployees.ToList())
            {
                decimal totalcommission = 0;

                var purchaseorders = from h in db.PurchaseOrders
                                     where h.EmployeeName == e.Email
                                     select h;

                foreach (PurchaseOrder po in purchaseorders)
                {
                    if (po.Date.Month == DateTime.Now.Month)//计算发薪日所在月份的销售总额
                        totalcommission += po.Total;
                }

                Trace.WriteLine(e.Email + ":" + (e.Salary + totalcommission * (decimal)e.CommisonRate - e.Deduction));
                ///*
                PayRoll p = new PayRoll();
                p.EmployeeName = e.Email;
                p.Salary = e.Salary + totalcommission * (decimal)e.CommisonRate - e.Deduction;
                p.PayDate = DateTime.Now;
                p.PaymentMethod = e.PaymentMethod;
                if (e.PaymentMethod == paymentmethod.MAIL)
                    p.MailAddress = e.MailAddress;
                if (e.PaymentMethod == paymentmethod.TRANSFER)
                {
                    p.BankName = e.BankName;
                    p.BankAccount = e.BankAccount;
                }
                db.PayRolls.Add(p);
                db.SaveChanges();
                //*/
                //如果员工标记为要删除，则付完薪水后删除
                if(e.isDeleted != null)
                {
                    db.Employees.Remove(e);
                    db.SaveChanges();
                }
            }
        }
    }

}