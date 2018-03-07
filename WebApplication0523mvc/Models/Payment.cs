using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication0523mvc.Models
{
    public class Payment
    {
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