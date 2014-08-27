namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TranBankStatLine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransactionBankStatementLine",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TransactionID = c.Int(nullable: false),
                        BankStatementLineID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Transaction", t => t.TransactionID, cascadeDelete: true)
                .ForeignKey("dbo.BankStatementLine", t => t.BankStatementLineID, cascadeDelete: true)
                .Index(t => t.TransactionID)
                .Index(t => t.BankStatementLineID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TransactionBankStatementLine", new[] { "BankStatementLineID" });
            DropIndex("dbo.TransactionBankStatementLine", new[] { "TransactionID" });
            DropForeignKey("dbo.TransactionBankStatementLine", "BankStatementLineID", "dbo.BankStatementLine");
            DropForeignKey("dbo.TransactionBankStatementLine", "TransactionID", "dbo.Transaction");
            DropTable("dbo.TransactionBankStatementLine");
        }
    }
}
