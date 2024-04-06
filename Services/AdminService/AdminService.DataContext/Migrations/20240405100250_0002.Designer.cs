﻿// <auto-generated />
using System;
using AdminService.DataContext.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AdminService.DataContext.Migrations
{
    [DbContext(typeof(Database.DataContext))]
    [Migration("20240405100250_0002")]
    partial class _0002
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AdminService.DataContext.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<DateTime?>("HandledAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages");
                });

            modelBuilder.Entity("AdminService.Domain.Models.ComplaintAggregate.Complaint", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ComplaintType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<bool>("IsHandled")
                        .HasColumnType("boolean")
                        .HasColumnName("is_handled");

                    b.Property<string>("SenderEmail")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("sender_email");

                    b.Property<string>("ViolatorEmail")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("violator_email");

                    b.HasKey("Id");

                    b.ToTable("complaints", (string)null);
                });

            modelBuilder.Entity("AdminService.Domain.Models.FeedbackAggregate.Feedback", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<string>("EmailFrom")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email_from");

                    b.Property<bool>("IsWatched")
                        .HasColumnType("boolean")
                        .HasColumnName("is_watched");

                    b.HasKey("Id");

                    b.ToTable("feedbacks", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}