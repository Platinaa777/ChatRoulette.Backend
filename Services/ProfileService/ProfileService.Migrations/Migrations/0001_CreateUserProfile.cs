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
                nick_name TEXT NOT NULL,
                email TEXT NOT NULL,
                age int,
                rating int,
                avatar text,
                achievements text,
                CONSTRAINT EMAIL_UNIQUE_CHECK UNIQUE (email),
                CONSTRAINT RATING_POSITIVE CHECK (rating >= 0));
        ");
        
        Execute.Sql(@"
            CREATE TABLE if not exists friends_link (
                  profile_id text,
                  friend_id text,
                  CONSTRAINT FRIEND_PAIR_UNIQUE UNIQUE (profile_id, friend_id));
        ");
    }

    public override void Down()
    {
        Execute.Sql(@"
            DROP TABLE if exists user_profiles;
            DROP TABLE if exists friends_link;
        ");
    }
}