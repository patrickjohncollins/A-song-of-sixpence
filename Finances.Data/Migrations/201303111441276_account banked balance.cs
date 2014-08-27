namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class accountbankedbalance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Account", "CurrentBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue:0));
            AddColumn("dbo.Account", "BankedBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue:0));
            DropColumn("dbo.Account", "Balance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Account", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Account", "BankedBalance");
            DropColumn("dbo.Account", "CurrentBalance");
        }
    }
}
