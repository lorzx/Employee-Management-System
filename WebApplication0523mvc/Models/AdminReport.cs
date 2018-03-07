using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication0523mvc.Models
{
    public class AdminReport
    {
        [Display(Name = "报告类型")]
        public reporttype ReportType { get; set; }

        [Display(Name = "起始日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "结束日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name ="员工姓名")]
        [Required]
        public string EmployeeName { get; set; }

        [Display(Name ="累计工时")]
        public int TotalHour { get; set; }

        [Display(Name ="年度累计收入")]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name ="年份")]
        public int Year { get; set; }

    }

    public enum reporttype { [Display(Name = "累计工时")]TOTALHOUR, [Display(Name = "当前全年工资")]YEARtoDATE }
}