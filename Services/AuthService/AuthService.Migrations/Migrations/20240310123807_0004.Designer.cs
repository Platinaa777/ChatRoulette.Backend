﻿// <auto-generated />
using System;
using AuthService.DataContext.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AuthService.Migrations.Migrations
{
    [DbContext(typeof(UserDb))]
    [Migration("20240310123807_0004")]
    partial class _0004
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AuthService.Domain.Models.UserAggregate.Entities.RefreshToken", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expired_at");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("boolean")
                        .HasColumnName("is_used");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("token");

                    b.HasKey("Id");

                    b.HasIndex("Token");

                    b.ToTable("tokens", (string)null);
                });

            modelBuilder.Entity("AuthService.Domain.Models.UserAggregate.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<bool>("IsSubmittedEmail")
                        .HasColumnType("boolean")
                        .HasColumnName("confirmation");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("salt");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.HasIndex("Email");

                    b.ToTable("users", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}