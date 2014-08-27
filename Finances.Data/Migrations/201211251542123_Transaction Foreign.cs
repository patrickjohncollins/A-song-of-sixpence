namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionForeign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "Foreign", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.Transaction", "ForeignDebit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "ForeignCredit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "ForeignCurrency", c => c.String(maxLength: 3));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transaction", "ForeignCurrency");
            DropColumn("dbo.Transaction", "ForeignCredit");
            DropColumn("dbo.Transaction", "ForeignDebit");
            DropColumn("dbo.Transaction", "Foreign");
        }
    }
}
