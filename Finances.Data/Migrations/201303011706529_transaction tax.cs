namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transactiontax : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "DebitExTax", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "DebitTax", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "CreditExTax", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "CreditTax", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transaction", "CreditTax");
            DropColumn("dbo.Transaction", "CreditExTax");
            DropColumn("dbo.Transaction", "DebitTax");
            DropColumn("dbo.Transaction", "DebitExTax");
        }
    }
}
