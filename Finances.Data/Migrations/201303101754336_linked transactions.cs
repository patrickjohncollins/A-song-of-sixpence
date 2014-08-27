namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class linkedtransactions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "ParentTransactionID", c => c.Int());
            AddForeignKey("dbo.Transaction", "ParentTransactionID", "dbo.Transaction", "ID");
            CreateIndex("dbo.Transaction", "ParentTransactionID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Transaction", new[] { "ParentTransactionID" });
            DropForeignKey("dbo.Transaction", "ParentTransactionID", "dbo.Transaction");
            DropColumn("dbo.Transaction", "ParentTransactionID");
        }
    }
}
