﻿// <auto-generated />
using System;
using Chat.DataContext.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Chat.DataContext.Migrations
{
    [DbContext(typeof(ChatDbContext))]
    partial class ChatDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Chat.Domain.Entities.ChatUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("ConnectionId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("connection_id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("PreviousParticipantEmails")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("peers_list");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("chat_users", (string)null);
                });

            modelBuilder.Entity("Chat.Domain.Entities.TwoSeatsRoom", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<DateTime?>("ClosedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("closed_at");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("PeerEmails")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("peers_emails");

                    b.HasKey("Id");

                    b.ToTable("rooms", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}