namespace WebApplication0523mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpayment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "BankName", c => c.String());
            AddColumn("dbo.Employees", "BankAccount", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "BankAccount");
            DropColumn("dbo.Employees", "BankName");
        }
    }
}
