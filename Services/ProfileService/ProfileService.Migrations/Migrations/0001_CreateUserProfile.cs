using FluentMigrator;

namespace ProfileService.Migrations.Migrations;


[Migration(1)]
public class CreateUserProfile : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE TABLE if not exists user_profiles (
                id text PRIMARY KEY,
                user_name TEXT NOT NULL,
                nick_name TEXT NOT NULL,
                email TEXT NOT NULL,
                CONSTRAINT EMAIL_UNIQUE_CHECK UNIQUE (email));
        ");
    }

    public override void Down()
    {
        Execute.Sql(@"
            DROP TABLE if exists user_profiles;
        ");
        
        
    }
}