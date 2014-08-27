namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transactioninvoicenum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "InvoiceNumber", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transaction", "InvoiceNumber");
        }
    }
}
