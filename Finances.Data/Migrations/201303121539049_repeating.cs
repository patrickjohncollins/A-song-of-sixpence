namespace Finances.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class repeating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "Repeat", c => c.Boolean(nullable: false, defaultValue:false));
            AddColumn("dbo.Transaction", "RepeatFrequency", c => c.Int());
            AddColumn("dbo.Transaction", "RepeatInterval", c => c.Int());
            AddColumn("dbo.Transaction", "RepeatUntil", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transaction", "RepeatUntil");
            DropColumn("dbo.Transaction", "RepeatInterval");
            DropColumn("dbo.Transaction", "RepeatFrequency");
            DropColumn("dbo.Transaction", "Repeat");
        }
    }
}
