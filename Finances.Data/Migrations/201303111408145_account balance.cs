namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class accountbalance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Account", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue:0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Account", "Balance");
        }
    }
}
