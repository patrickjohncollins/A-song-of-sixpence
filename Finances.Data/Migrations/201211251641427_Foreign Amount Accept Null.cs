namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignAmountAcceptNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transaction", "ForeignDebit", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Transaction", "ForeignCredit", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transaction", "ForeignCredit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Transaction", "ForeignDebit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
