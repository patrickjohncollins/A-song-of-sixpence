namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transactioninvoicedate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "InvoiceDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transaction", "InvoiceDate");
        }
    }
}
