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
    [Migration("20240410085821_AddAttributeAndCreateTablePassports")]
    partial class AddAttributeAndCreateTablePassports
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
                    b.Property<string>("TelegramTag")
                        .HasMaxLength(33)
                        .HasColumnType("character varying(33)")
                        .HasColumnName("telegramtag");

                    b.Property<string>("AdminLink")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("adminlink");

                    b.HasKey("TelegramTag");

                    b.ToTable("admins");
                });

            modelBuilder.Entity("Fiit_passport.Models.Passport", b =>
                {
                    b.Property<string>("SessionId")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("AcceptanceCriteria")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CopiesNumber")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Goal")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MeetingLocation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("OrdererName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProjectDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Result")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("SessionId");

                    b.ToTable("passports");
                });
#pragma warning restore 612, 618
        }
    }
}
