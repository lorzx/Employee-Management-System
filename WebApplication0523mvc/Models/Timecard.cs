using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication0523mvc.Models
{
    public class Timecard
    {
        public int ID { get; set; }
        public string EmployeeName { get; set; }

        [Display(Name = "开始日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "结束日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "项目编号")]
        [Required]
        public string Chargenumber { get; set; }

        [Display(Name = "时长")]
        [Range(0,24)]
        [Required]
        public int Hours { get; set; }

        [Display(Name = "工作日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime Date { get; set; }

        [Display(Name = "提交日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SubmitDate { get; set; }

        [Display(Name = "提交情况")]
        public string isSubmitted { get; set; }
        //[StringLength(60, MinimumLength = 3)]
        //[Required]
        //public string Title { get; set; }

        //[Display(Name = "Release Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //public DateTime ReleaseDate { get; set; }
        //public string Genre { get; set; }

        //[Range(1, 100)]
        //[DataType(DataType.Currency)]
        //public decimal Price { get; set; }
    }

    public class ChargeNumber
    {
        public int ID { get; set; }
        public string chargenumber { get; set; }
    }
}