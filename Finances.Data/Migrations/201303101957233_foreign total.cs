namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foreigntotal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "ForeignTotal", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transaction", "ForeignTotal");
        }
    }
}
