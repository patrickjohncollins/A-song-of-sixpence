namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transtotaltax : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "TotalExTax", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "TotalTax", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transaction", "TotalTax");
            DropColumn("dbo.Transaction", "TotalExTax");
        }
    }
}
