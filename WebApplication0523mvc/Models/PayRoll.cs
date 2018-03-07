using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication0523mvc.Models
{
    public class PayRoll
    {
        public int ID { get; set; }

        public string EmployeeName { get; set; }

        [Display(Name = "薪水")]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name = "交易日期")]
        [DataType(DataType.Date)]
        public DateTime PayDate { get; set; }

        [Display(Name = "发放方式")]
        public paymentmethod PaymentMethod { get; set; }

        [Display(Name = "邮寄地址")]
        public string MailAddress { get; set; }

        [Display(Name = "银行名称")]
        public string BankName { get; set; }

        [Display(Name = "银行账号")]
        public string BankAccount { get; set; }
    }
}