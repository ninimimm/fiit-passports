﻿// <auto-generated />
using Fiit_passport.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fiit_passport.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240424163333_AddEnumStatusFromPassport")]
    partial class AddEnumStatusFromPassport
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Fiit_passport.Models.Admin", b =>
                {
                    b.Property<string>("AdminTelegramTag")
                        .HasMaxLength(33)
                        .HasColumnType("character varying(33)")
                        .HasColumnName("admin_telegram_tag");

                    b.Property<string>("AdminLink")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("admin_link");

                    b.HasKey("AdminTelegramTag");

                    b.ToTable("admins");
                });

            modelBuilder.Entity("Fiit_passport.Models.ConnectId", b =>
                {
                    b.Property<string>("UserTelegramTag")
                        .HasMaxLength(33)
                        .HasColumnType("character varying(33)")
                        .HasColumnName("user_telegram_tag");

                    b.Property<string>("UserTelegramId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("user_telegram_id");

                    b.HasKey("UserTelegramTag");

                    b.ToTable("connect_ids");
                });

            modelBuilder.Entity("Fiit_passport.Models.Passport", b =>
                {
                    b.Property<string>("SessionId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)")
                        .HasColumnName("session_id");

                    b.Property<string>("AcceptanceCriteria")
                        .HasMaxLength(100000)
                        .HasColumnType("character varying(100000)")
                        .HasColumnName("error_message");

                    b.Property<string>("AuthenticatedTelegramTag")
                        .HasColumnType("text")
                        .HasColumnName("authenticated_telegram_tag");

                    b.Property<int>("CopiesNumber")
                        .HasColumnType("integer")
                        .HasColumnName("copies_number");

                    b.Property<string>("Email")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("email");

                    b.Property<string>("Goal")
                        .HasMaxLength(100000)
                        .HasColumnType("character varying(100000)")
                        .HasColumnName("goal");

                    b.Property<string>("MeetingLocation")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("meeting_location");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<string>("OrdererName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("orderer_name");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("phone_number");

                    b.Property<string>("ProjectDescription")
                        .HasMaxLength(100000)
                        .HasColumnType("character varying(100000)")
                        .HasColumnName("project_description");

                    b.Property<string>("ProjectName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("project_name");

                    b.Property<string>("Result")
                        .HasMaxLength(100000)
                        .HasColumnType("character varying(100000)")
                        .HasColumnName("result");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<string>("Surname")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("surname");

                    b.Property<string>("TelegramTag")
                        .HasMaxLength(33)
                        .HasColumnType("character varying(33)")
                        .HasColumnName("telegram_tag");

                    b.HasKey("SessionId");

                    b.ToTable("passports");
                });
#pragma warning restore 612, 618
        }
    }
}
