namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BankRecon : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TransactionBankStatementLine", "TransactionID", "dbo.Transaction");
            DropForeignKey("dbo.TransactionBankStatementLine", "BankStatementLineID", "dbo.BankStatementLine");
            DropIndex("dbo.TransactionBankStatementLine", new[] { "TransactionID" });
            DropIndex("dbo.TransactionBankStatementLine", new[] { "BankStatementLineID" });
            CreateTable(
                "dbo.BankReconciliation",
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
            
            DropTable("dbo.TransactionBankStatementLine");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TransactionBankStatementLine",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TransactionID = c.Int(nullable: false),
                        BankStatementLineID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropIndex("dbo.BankReconciliation", new[] { "BankStatementLineID" });
            DropIndex("dbo.BankReconciliation", new[] { "TransactionID" });
            DropForeignKey("dbo.BankReconciliation", "BankStatementLineID", "dbo.BankStatementLine");
            DropForeignKey("dbo.BankReconciliation", "TransactionID", "dbo.Transaction");
            DropTable("dbo.BankReconciliation");
            CreateIndex("dbo.TransactionBankStatementLine", "BankStatementLineID");
            CreateIndex("dbo.TransactionBankStatementLine", "TransactionID");
            AddForeignKey("dbo.TransactionBankStatementLine", "BankStatementLineID", "dbo.BankStatementLine", "ID", cascadeDelete: true);
            AddForeignKey("dbo.TransactionBankStatementLine", "TransactionID", "dbo.Transaction", "ID", cascadeDelete: true);
        }
    }
}
