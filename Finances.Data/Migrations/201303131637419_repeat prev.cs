namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class repeatprev : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "PreviousTransactionID", c => c.Int());
            AddForeignKey("dbo.Transaction", "PreviousTransactionID", "dbo.Transaction", "ID");
            CreateIndex("dbo.Transaction", "PreviousTransactionID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Transaction", new[] { "PreviousTransactionID" });
            DropForeignKey("dbo.Transaction", "PreviousTransactionID", "dbo.Transaction");
            DropColumn("dbo.Transaction", "PreviousTransactionID");
        }
    }
}
