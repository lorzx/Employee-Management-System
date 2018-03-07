namespace WebApplication0523mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class beta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PayRolls", "PaymentMethod", c => c.Int(nullable: false));
            AddColumn("dbo.PayRolls", "MailAddress", c => c.String());
            AddColumn("dbo.PayRolls", "BankName", c => c.String());
            AddColumn("dbo.PayRolls", "BankAccount", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PayRolls", "BankAccount");
            DropColumn("dbo.PayRolls", "BankName");
            DropColumn("dbo.PayRolls", "MailAddress");
            DropColumn("dbo.PayRolls", "PaymentMethod");
        }
    }
}
