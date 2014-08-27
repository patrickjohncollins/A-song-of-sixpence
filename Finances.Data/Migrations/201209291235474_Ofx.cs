namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ofx : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankAccount",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AccountID = c.String(nullable: false),
                        AccountType = c.String(nullable: false),
                        AccountKey = c.String(),
                        BankAccountType = c.String(),
                        BankID = c.String(),
                        BranchID = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.BankStatement",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        LedgerBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LedgerBalanceDate = c.DateTime(nullable: false),
                        AvaliableBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AvaliableBalanceDate = c.DateTime(nullable: false),
                        Account_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BankAccount", t => t.Account_ID, cascadeDelete: true)
                .Index(t => t.Account_ID);
            
            CreateTable(
                "dbo.BankStatementLine",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TransType = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionID = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Memo = c.String(),
                        IncorrectTransactionID = c.String(),
                        TransactionCorrectionAction = c.String(),
                        ServerTransactionID = c.String(),
                        CheckNum = c.String(),
                        ReferenceNumber = c.String(),
                        Sic = c.String(),
                        PayeeID = c.String(),
                        Currency = c.String(),
                        Statement_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BankStatement", t => t.Statement_ID)
                .Index(t => t.Statement_ID);
            
            AlterColumn("dbo.Transaction", "Banked", c => c.DateTime());
            AlterColumn("dbo.Transaction", "ChequeNumber", c => c.Int());
            AlterColumn("dbo.Transaction", "Category", c => c.String(maxLength: 255));
            AlterColumn("dbo.Transaction", "Payee", c => c.String(maxLength: 255));
            AlterColumn("dbo.Transaction", "Nature", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropIndex("dbo.BankStatementLine", new[] { "Statement_ID" });
            DropIndex("dbo.BankStatement", new[] { "Account_ID" });
            DropForeignKey("dbo.BankStatementLine", "Statement_ID", "dbo.BankStatement");
            DropForeignKey("dbo.BankStatement", "Account_ID", "dbo.BankAccount");
            AlterColumn("dbo.Transaction", "Nature", c => c.String());
            AlterColumn("dbo.Transaction", "Payee", c => c.String());
            AlterColumn("dbo.Transaction", "Category", c => c.String());
            AlterColumn("dbo.Transaction", "ChequeNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.Transaction", "Banked", c => c.DateTime(nullable: false));
            DropTable("dbo.BankStatementLine");
            DropTable("dbo.BankStatement");
            DropTable("dbo.BankAccount");
            RenameTable(name: "dbo.Transaction", newName: "Transactions");
        }
    }
}
