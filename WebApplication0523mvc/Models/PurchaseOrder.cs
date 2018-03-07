using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication0523mvc.Models
{
    public class PurchaseOrder
    {
        [Display(Name = "清单ID")]
        public int ID { get; set; }

        public string EmployeeName { get; set; }

        [Display(Name = "销售日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime Date { get; set; }

        [Display(Name = "顾客姓名")]
        [Required]
        public string CustomerName { get; set; }

        [Display(Name = "账单邮寄地址")]
        [Required]
        public string BillingAddress { get; set; }

        [Display(Name = "所购商品")]
        [Required]
        public string Product { get; set; }

        [Display(Name = "销售金额")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Total { get; set; }

        [Display(Name = "订单状况")]
        public string isClosed { get; set; }
    }
}