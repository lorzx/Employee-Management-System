namespace WebApplication0523mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstedition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "isDeleted", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "isDeleted");
        }
    }
}
