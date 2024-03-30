using FluentMigrator;

namespace ProfileService.Migrations.Migrations;

[Migration(3)]
public class CreateInvitationTable : Migration 
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE TABLE invitations (
            id text PRIMARY KEY,
            sender_id TEXT NOT NULL,
            receiver_id TEXT NOT NULL,
            status TEXT NOT NULL DEFAULT 'pending', -- может быть 'pending', 'accepted' или 'rejected'
            sent_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            CONSTRAINT unique_request UNIQUE (sender_id, receiver_id)
        );");
    }

    public override void Down()
    {
        Execute.Sql(@"
            DROP TABLE if exists invitations;
        ");
    }
}