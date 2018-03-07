using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace WebApplication0523mvc.Models
{
    public class Movie
    {
        public int ID { get; set; }

        [StringLength(60,MinimumLength =3)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }

        [Range(1,100)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }

    public class MovieDBContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Timecard> Timecards { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ChargeNumber> ChargeNumbers { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PayRoll> PayRolls { get; set; }
    }
}