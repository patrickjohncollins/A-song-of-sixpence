namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ofxmoneytype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BankStatement", "LedgerBalance", c => c.Decimal(nullable: false, storeType: "money"));
            AlterColumn("dbo.BankStatement", "AvaliableBalance", c => c.Decimal(nullable: false, storeType: "money"));
            AlterColumn("dbo.BankStatementLine", "Amount", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BankStatementLine", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.BankStatement", "AvaliableBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.BankStatement", "LedgerBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
