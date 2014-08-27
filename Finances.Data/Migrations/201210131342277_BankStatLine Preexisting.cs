namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BankStatLinePreexisting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankStatementLine", "Preexisting", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BankStatementLine", "Preexisting");
        }
    }
}
