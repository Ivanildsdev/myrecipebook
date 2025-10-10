using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_USER)]
    public class Version0000001 : VersionBase
    {
        public override void Up()
        {
            Create.Table("Users")
            .WithColumn("Name").AsString(50).NotNullable().Unique()
            .WithColumn("Email").AsString(100).NotNullable().Unique()
            .WithColumn("Password").AsString(2000).NotNullable();
        }
    }
}
