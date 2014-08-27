namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transactionorder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "OrderID", c => c.Int(nullable: true));
            AddForeignKey("dbo.Transaction", "OrderID", "dbo.Order", "ID", cascadeDelete: true);
            CreateIndex("dbo.Transaction", "OrderID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Transaction", new[] { "OrderID" });
            DropForeignKey("dbo.Transaction", "OrderID", "dbo.Order");
            DropColumn("dbo.Transaction", "OrderID");
        }
    }
}
