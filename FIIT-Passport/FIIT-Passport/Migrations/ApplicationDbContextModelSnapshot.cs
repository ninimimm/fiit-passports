﻿// <auto-generated />
using Fiit_passport.Databased;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fiit_passport.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("Fiit_passport.Models.Connect", b =>
                {
                    b.Property<string>("TelegramTag")
                        .HasMaxLength(33)
                        .HasColumnType("character varying(33)")
                        .HasColumnName("telegram_tag");

                    b.Property<string>("SessionId")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("session_id");

                    b.HasKey("TelegramTag");

                    b.HasIndex("SessionId")
                        .IsUnique();

                    b.ToTable("connects");
                });

            modelBuilder.Entity("Fiit_passport.Models.Passport", b =>
                {
                    b.Property<string>("SessionId")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("session_id");

                    b.Property<string>("AcceptanceCriteria")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("error_message");

                    b.Property<int>("CopiesNumber")
                        .HasColumnType("integer")
                        .HasColumnName("copies_number");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Goal")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("goal");

                    b.Property<string>("MeetingLocation")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("meeting_location");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<string>("OrdererName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("orderer_name");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<string>("ProjectDescription")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("project_description");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("project_name");

                    b.Property<string>("Result")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("result");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("surname");

                    b.HasKey("SessionId");

                    b.ToTable("passports");
                });

            modelBuilder.Entity("Fiit_passport.Models.Connect", b =>
                {
                    b.HasOne("Fiit_passport.Models.Passport", "Passport")
                        .WithOne("Connect")
                        .HasForeignKey("Fiit_passport.Models.Connect", "SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Passport");
                });

            modelBuilder.Entity("Fiit_passport.Models.Passport", b =>
                {
                    b.Navigation("Connect")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
