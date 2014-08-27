namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transactionaccount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Transaction", "AccountID", c => c.Int(nullable: true));
            AddForeignKey("dbo.Transaction", "AccountID", "dbo.Account", "ID", cascadeDelete: true);
            CreateIndex("dbo.Transaction", "AccountID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Transaction", new[] { "AccountID" });
            DropForeignKey("dbo.Transaction", "AccountID", "dbo.Account");
            DropColumn("dbo.Transaction", "AccountID");
            DropTable("dbo.Account");
        }
    }
}
