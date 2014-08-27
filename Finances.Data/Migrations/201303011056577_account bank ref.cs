namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class accountbankref : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Account", "BankAccountID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Account", "BankAccountID");
        }
    }
}
