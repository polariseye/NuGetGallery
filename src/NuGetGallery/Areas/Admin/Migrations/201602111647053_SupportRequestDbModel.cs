namespace NuGetGallery.Areas.Admin
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SupportRequestDbModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Admins",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        PagerDutyUsername = c.String(nullable: false, maxLength: 255, unicode: false),
                        GalleryUsername = c.String(nullable: false, maxLength: 255, unicode: false),
                        AccessDisabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .Index(t => t.PagerDutyUsername)
                .Index(t => t.GalleryUsername);

            CreateTable(
                "History",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        EntryDate = c.DateTime(nullable: false),
                        EditedBy = c.String(unicode: false),
                        Comments = c.String(unicode: false),
                        IssueId = c.Int(nullable: false),
                        IssueStatusId = c.Int(nullable: false),
                        AssignedToId = c.Int(),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("Admins", t => t.AssignedToId)
                .ForeignKey("Issues", t => t.IssueId, cascadeDelete: true)
                .Index(t => t.IssueId)
                .Index(t => t.AssignedToId);

            CreateTable(
                "Issues",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        CreatedBy = c.String(maxLength: 50, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        IssueTitle = c.String(nullable: false, maxLength: 1000, unicode: false),
                        Details = c.String(nullable: false, unicode: false),
                        SiteRoot = c.String(unicode: false),
                        PackageId = c.String(nullable: false, maxLength: 300, unicode: false),
                        PackageVersion = c.String(nullable: false, maxLength: 300, unicode: false),
                        OwnerEmail = c.String(nullable: false, maxLength: 100, unicode: false),
                        Reason = c.String(maxLength: 100, unicode: false),
                        AssignedToId = c.Int(),
                        IssueStatusId = c.Int(nullable: false),
                        PackageRegistrationKey = c.Int(),
                        UserKey = c.Int(),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("IssueStatus", t => t.IssueStatusId, cascadeDelete: true)
                .ForeignKey("Admins", t => t.AssignedToId)
                .Index(t => t.AssignedToId)
                .Index(t => t.IssueStatusId);

            CreateTable(
                "IssueStatus",
                c => new
                    {
                        Key = c.Int(nullable: false),
                        Name = c.String(maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.Key)
                .Index(t => t.Name);

            Sql("INSERT INTO IssueStatus VALUES (0, 'New')");
            Sql("INSERT INTO IssueStatus VALUES (1, 'Working')");
            Sql("INSERT INTO IssueStatus VALUES (2, 'Waiting for customer')");
            Sql("INSERT INTO IssueStatus VALUES (3, 'Resolved')");
        }

        public override void Down()
        {
            DropForeignKey("Issues", "AssignedToId", "Admins");
            DropForeignKey("Issues", "IssueStatusId", "IssueStatus");
            DropForeignKey("History", "IssueId", "Issues");
            DropForeignKey("History", "AssignedToId", "Admins");
            DropIndex("IssueStatus", new[] { "Name" });
            DropIndex("Issues", new[] { "IssueStatusId" });
            DropIndex("Issues", new[] { "AssignedToId" });
            DropIndex("History", new[] { "AssignedToId" });
            DropIndex("History", new[] { "IssueId" });
            DropIndex("Admins", new[] { "GalleryUsername" });
            DropIndex("Admins", new[] { "PagerDutyUsername" });
            DropTable("IssueStatus");
            DropTable("Issues");
            DropTable("History");
            DropTable("Admins");
        }
    }
}
