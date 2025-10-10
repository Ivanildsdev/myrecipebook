using FluentMigrator;
using FluentMigrator.Builders.Create.Table;


namespace MyRecipeBook.Infrastructure.Migrations.Versions
{
    public abstract class VersionBase : ForwardOnlyMigration
    {
        protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string tableName)
        {
            // if (!Schema.Table(tableName).Exists())
            return Create.Table(tableName)
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("CreatedOn").AsDateTime().NotNullable()//.WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("Active").AsBoolean().NotNullable();
        }
    }
}
