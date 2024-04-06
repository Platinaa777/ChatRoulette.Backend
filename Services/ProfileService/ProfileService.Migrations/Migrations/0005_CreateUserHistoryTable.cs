using FluentMigrator;

namespace ProfileService.Migrations.Migrations;

[Migration(5)]
public class CreateUserHistoryTable : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE TABLE if not exists user_histories (
                history_id text PRIMARY KEY,
                user_id text,
                doom_points int,
                avatar_points int,
                CONSTRAINT HISTORY_USER_ID UNIQUE (user_id),
                CONSTRAINT DOOM_POINTS_POSITIVE CHECK (doom_points >= 0),
                CONSTRAINT AVATAR_POINTS_POSITIVE CHECK (avatar_points >= 0));
        ");
    }

    public override void Down()
    {
        Execute.Sql(@"
            DROP TABLE if exists user_histories;
        ");
    }
}