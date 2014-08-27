namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class accounttransaction : DbMigration
    {
        public override void Up()
        {

            

            DropForeignKey("dbo.Transaction", "AccountID", "dbo.Account");
            DropForeignKey("dbo.Transaction", "ParentTransactionID", "dbo.Transaction");
            DropForeignKey("dbo.BankReconciliation", "TransactionID", "dbo.Transaction");

            DropIndex("dbo.Transaction", new[] { "AccountID" });
            DropIndex("dbo.Transaction", new[] { "ParentTransactionID" });
            DropIndex("dbo.BankReconciliation", new[] { "TransactionID" });

            CreateTable(
                "dbo.AccountTransaction",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TransactionID = c.Int(nullable: false),
                        AccountID = c.Int(nullable: false),
                        Banked = c.DateTime(),
                        DebitExTax = c.Decimal(precision: 18, scale: 2),
                        DebitTax = c.Decimal(precision: 18, scale: 2),
                        Debit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditExTax = c.Decimal(precision: 18, scale: 2),
                        CreditTax = c.Decimal(precision: 18, scale: 2),
                        Credit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalExTax = c.Decimal(precision: 18, scale: 2),
                        TotalTax = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForeignDebit = c.Decimal(precision: 18, scale: 2),
                        ForeignCredit = c.Decimal(precision: 18, scale: 2),
                        ForeignTotal = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Transaction", t => t.TransactionID, cascadeDelete: true)
                .ForeignKey("dbo.Account", t => t.AccountID, cascadeDelete: true)
                .Index(t => t.TransactionID)
                .Index(t => t.AccountID);

            AddColumn("dbo.AccountTransaction", "TempOldTransactionID", c => c.Int(nullable: false));

            Sql("INSERT INTO AccountTransaction " +
                "SELECT CASE WHEN ParentTransactionID IS NULL THEN ID ELSE ParentTransactionID END AS TransactionID , AccountID , Banked , DebitExTax , DebitTax , Debit , CreditExTax , CreditTax , Credit , TotalExTax , TotalTax , Total , Balance , ForeignDebit , ForeignCredit , ForeignTotal , ID " +
                "FROM [Transaction]");

            AddColumn("dbo.BankReconciliation", "AccountTransactionID", c => c.Int(nullable: false, defaultValue: 0));

            Sql("UPDATE BankReconciliation SET AccountTransactionID = ( " +
                "SELECT ID FROM AccountTransaction WHERE TempOldTransactionID = BankReconciliation.TransactionID" +
                ")");

            DropColumn("dbo.AccountTransaction", "TempOldTransactionID");
            DropColumn("dbo.BankReconciliation", "TransactionID");

            AddForeignKey("dbo.BankReconciliation", "AccountTransactionID", "dbo.AccountTransaction", "ID", cascadeDelete: true);
            CreateIndex("dbo.BankReconciliation", "AccountTransactionID");

            Sql("DELETE FROM [Transaction] " +
                "WHERE ParentTransactionID IS NOT NULL");

            

            DropColumn("dbo.Transaction", "AccountID");
            DropColumn("dbo.Transaction", "Banked");
            DropColumn("dbo.Transaction", "DebitExTax");
            DropColumn("dbo.Transaction", "DebitTax");
            DropColumn("dbo.Transaction", "Debit");
            DropColumn("dbo.Transaction", "CreditExTax");
            DropColumn("dbo.Transaction", "CreditTax");
            DropColumn("dbo.Transaction", "Credit");
            DropColumn("dbo.Transaction", "TotalExTax");
            DropColumn("dbo.Transaction", "TotalTax");
            DropColumn("dbo.Transaction", "Total");
            DropColumn("dbo.Transaction", "Balance");
            DropColumn("dbo.Transaction", "ForeignDebit");
            DropColumn("dbo.Transaction", "ForeignCredit");
            DropColumn("dbo.Transaction", "ForeignTotal");
            DropColumn("dbo.Transaction", "ParentTransactionID");

        }
        
        public override void Down()
        {
            AddColumn("dbo.BankReconciliation", "TransactionID", c => c.Int(nullable: false));
            AddColumn("dbo.Transaction", "ParentTransactionID", c => c.Int());
            AddColumn("dbo.Transaction", "ForeignTotal", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "ForeignCredit", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "ForeignDebit", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "TotalTax", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "TotalExTax", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "Credit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "CreditTax", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "CreditExTax", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "Debit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "DebitTax", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "DebitExTax", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Transaction", "Banked", c => c.DateTime());
            DropIndex("dbo.BankReconciliation", new[] { "AccountTransactionID" });
            DropIndex("dbo.AccountTransaction", new[] { "AccountID" });
            DropIndex("dbo.AccountTransaction", new[] { "TransactionID" });
            DropIndex("dbo.Transaction", new[] { "Account_ID" });
            DropForeignKey("dbo.BankReconciliation", "AccountTransactionID", "dbo.AccountTransaction");
            DropForeignKey("dbo.AccountTransaction", "AccountID", "dbo.Account");
            DropForeignKey("dbo.AccountTransaction", "TransactionID", "dbo.Transaction");
            DropForeignKey("dbo.Transaction", "Account_ID", "dbo.Account");
            DropColumn("dbo.BankReconciliation", "AccountTransactionID");
            DropTable("dbo.AccountTransaction");
            RenameColumn(table: "dbo.Transaction", name: "Account_ID", newName: "AccountID");
            CreateIndex("dbo.BankReconciliation", "TransactionID");
            CreateIndex("dbo.Transaction", "ParentTransactionID");
            CreateIndex("dbo.Transaction", "AccountID");
            AddForeignKey("dbo.BankReconciliation", "TransactionID", "dbo.Transaction", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Transaction", "ParentTransactionID", "dbo.Transaction", "ID");
            AddForeignKey("dbo.Transaction", "AccountID", "dbo.Account", "ID", cascadeDelete: true);
        }
    }
}
