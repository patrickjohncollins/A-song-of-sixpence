namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignExchangeRate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transaction", "ExchangeRate", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transaction", "ExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
