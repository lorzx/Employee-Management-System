using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication0523mvc.Models
{
    public class Employee
    {
        public int ID { get; set; }

        [Display(Name = "邮箱")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Display(Name = "姓名")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "工种")]
        [Required]
        public position Position { get; set; }

        [Display(Name = "邮寄地址")]
        public string MailAddress { get; set; }

        [Display(Name = "社保号")]
        public string SSN { get; set; }

        [Display(Name = "税费")]
        [DataType(DataType.Currency)]
        public decimal Deduction { get; set; }

        [Display(Name = "手机号")]
        [StringLength(11, MinimumLength = 11)]
        public string Phone { get; set; }

        [Display(Name = "时薪")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal HourRate { get; set; }

        [Display(Name = "月薪")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Salary { get; set; }

        [Display(Name = "提成比例")]
        [Range(0,1)]
        [Required]
        public float CommisonRate { get; set; }

        [Display(Name = "工作时限")]
        [Required]
        public int HourLimit { get; set; }

        [Display(Name = "工资发放方式")]
        [Required]
        public paymentmethod PaymentMethod { get; set; }

        [Display(Name = "年假")]
        [Range(0, 365)]
        [Required]
        public int Vacation { get; set; }

        public string BankName { get; set; }

        public string BankAccount { get; set; }

        [Display(Name = "离职")]
        public string isDeleted { get; set; }
    }

    public enum position { HOUR, SALARY, COMMISION }

    public enum paymentmethod { PICKUP, MAIL, TRANSFER }
}