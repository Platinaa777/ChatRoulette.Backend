using FluentMigrator;

namespace ProfileService.Migrations.Migrations;

[Migration(4)]
public class CreateOutboxMessageTable : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE TABLE if not exists outbox_messages (
                id UUID PRIMARY KEY,
                type VARCHAR(255) NOT NULL,
                content TEXT NOT NULL,
                started_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,
                handled_at TIMESTAMP WITHOUT TIME ZONE,
                error TEXT);
        ");
    }

    public override void Down()
    {
        Execute.Sql(@"
            DROP TABLE if exists outbox_messages;
        ");
    }
}