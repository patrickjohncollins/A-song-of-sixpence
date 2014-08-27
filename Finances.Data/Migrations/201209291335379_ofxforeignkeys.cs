namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ofxforeignkeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BankStatementLine", "Statement_ID", "dbo.BankStatement");
            DropIndex("dbo.BankStatementLine", new[] { "Statement_ID" });
            RenameColumn(table: "dbo.BankStatement", name: "Account_ID", newName: "BankAccountID");
            RenameColumn(table: "dbo.BankStatementLine", name: "Statement_ID", newName: "BankStatementID");
            AddForeignKey("dbo.BankStatementLine", "BankStatementID", "dbo.BankStatement", "ID", cascadeDelete: true);
            CreateIndex("dbo.BankStatementLine", "BankStatementID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BankStatementLine", new[] { "BankStatementID" });
            DropForeignKey("dbo.BankStatementLine", "BankStatementID", "dbo.BankStatement");
            RenameColumn(table: "dbo.BankStatementLine", name: "BankStatementID", newName: "Statement_ID");
            RenameColumn(table: "dbo.BankStatement", name: "BankAccountID", newName: "Account_ID");
            CreateIndex("dbo.BankStatementLine", "Statement_ID");
            AddForeignKey("dbo.BankStatementLine", "Statement_ID", "dbo.BankStatement", "ID");
        }
    }
}
