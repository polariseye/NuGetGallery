using System.Data.Entity.Migrations;

namespace NuGetGallery.Areas.Admin
{
    public partial class PackageInfoIsOptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Issues", "PackageId", c => c.String(maxLength: 300, unicode: false));
            AlterColumn("Issues", "PackageVersion", c => c.String(maxLength: 300, unicode: false));
        }

        public override void Down()
        {
            AlterColumn("Issues", "PackageVersion", c => c.String(nullable: false, maxLength: 300, unicode: false));
            AlterColumn("Issues", "PackageId", c => c.String(nullable: false, maxLength: 300, unicode: false));
        }
    }
}
