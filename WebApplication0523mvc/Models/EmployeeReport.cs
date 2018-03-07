using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication0523mvc.Models
{
    public class EmployeeReport
    {
        [Display(Name = "报告类型")]
        public EMreporttype ReportType { get; set; }

        [Display(Name = "起始日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "结束日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "项目编号")]
        public string Chargenumber { get; set; }

        [Display(Name = "累计工时")]
        public int TotalHour { get; set; }

        [Display(Name = "年度累计收入")]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name = "年份")]
        public int Year { get; set; }

        [Display(Name ="员工姓名")]
        public string Name { get; set; }

        [Display(Name = "休假天数")]
        public int Vacation { get; set; }
    }

    public enum EMreporttype
    {
        [Display(Name = "累计工时")]
        TOTALHOUR,
        [Display(Name = "项目累计工时")]
        HOURSforPROJECT,
        [Display(Name = "剩余假期")]
        VACATION,
        [Display(Name = "当前全年工资")]
        YEARtoDATE
    }
}