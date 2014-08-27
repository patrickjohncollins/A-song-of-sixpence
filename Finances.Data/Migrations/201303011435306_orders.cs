namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Client = c.String(nullable: false, maxLength: 255),
                        EndClient = c.String(maxLength: 255),
                        Title = c.String(nullable: false, maxLength: 255),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Days = c.Int(nullable: false),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Fee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Travel = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        State = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Order");
        }
    }
}
