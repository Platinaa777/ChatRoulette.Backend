using FluentMigrator;

namespace ProfileService.Migrations.Migrations;

[Migration(2)]
public class CreateIdxEmailProfile : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE INDEX if not exists idx_email_profile
            ON user_profiles(email);
        ");
    }

    public override void Down()
    {
        Execute.Sql(@"
            DROP INDEX IF EXISTS idx_email_profile
        ");
    }
}