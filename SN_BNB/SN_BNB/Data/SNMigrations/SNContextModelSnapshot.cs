﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SN_BNB.Data;

namespace SN_BNB.Data.SNMigrations
{
    [DbContext(typeof(SNContext))]
    partial class SNContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("SN")
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SN_BNB.Models.AssignedMatchPlayer", b =>
                {
                    b.Property<int>("MatchID");

                    b.Property<int>("PlayerID");

                    b.HasKey("MatchID", "PlayerID");

                    b.HasIndex("PlayerID");

                    b.ToTable("AssignedMatchPlayer");
                });

            modelBuilder.Entity("SN_BNB.Models.Division", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DivisionName")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("DivisionName")
                        .IsUnique();

                    b.ToTable("Divisions");
                });

            modelBuilder.Entity("SN_BNB.Models.Fixture", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AwayScore");

                    b.Property<double>("BonusPoint");

                    b.Property<DateTime>("FixtureDateTime");

                    b.Property<int>("HomeScore");

                    b.Property<int?>("LocationID");

                    b.Property<int>("Season_idSeason");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int>("idAwayTeam");

                    b.Property<int>("idHomeTeam");

                    b.Property<int>("location_locationId");

                    b.HasKey("ID");

                    b.HasIndex("LocationID");

                    b.HasIndex("Season_idSeason");

                    b.HasIndex("idAwayTeam");

                    b.HasIndex("idHomeTeam");

                    b.ToTable("Fixtures");
                });

            modelBuilder.Entity("SN_BNB.Models.Location", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LocationBuildingNumber");

                    b.Property<string>("LocationCity")
                        .IsRequired();

                    b.Property<string>("LocationName")
                        .IsRequired();

                    b.Property<string>("LocationStreet")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("SN_BNB.Models.Match", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssignedMatchPlayerID");

                    b.Property<int>("FixtureID");

                    b.Property<int>("MatchPosition");

                    b.Property<TimeSpan>("MatchTime");

                    b.Property<int>("Player1Score");

                    b.Property<int>("Player2Score");

                    b.Property<int?>("PlayerID");

                    b.HasKey("ID");

                    b.HasIndex("FixtureID");

                    b.HasIndex("PlayerID");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("SN_BNB.Models.News", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(5000);

                    b.Property<DateTime>("Date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("ID");

                    b.ToTable("News");
                });

            modelBuilder.Entity("SN_BNB.Models.Player", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Against");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(45);

                    b.Property<int>("For");

                    b.Property<string>("Gender");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("Loss");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(45);

                    b.Property<long>("Phone");

                    b.Property<int>("Played");

                    b.Property<bool>("PlayerWantsInfoHidden");

                    b.Property<int>("Points");

                    b.Property<int>("Position");

                    b.Property<int>("TeamID");

                    b.Property<int>("Win");

                    b.HasKey("ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("TeamID");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("SN_BNB.Models.Season", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("SeasonEnd");

                    b.Property<DateTime>("SeasonStart");

                    b.Property<string>("Season_Title")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("SN_BNB.Models.Season_has_Team", b =>
                {
                    b.Property<int>("TeamID");

                    b.Property<int>("SeasonID");

                    b.HasKey("TeamID", "SeasonID");

                    b.HasIndex("SeasonID");

                    b.ToTable("SeasonTeams");
                });

            modelBuilder.Entity("SN_BNB.Models.Team", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DivisionID");

                    b.Property<string>("TeamBio");

                    b.Property<DateTime?>("TeamCreatedOn");

                    b.Property<int>("TeamLosses");

                    b.Property<string>("TeamName")
                        .IsRequired();

                    b.Property<int>("TeamPoints");

                    b.Property<int>("TeamWins");

                    b.HasKey("ID");

                    b.HasIndex("DivisionID");

                    b.HasIndex("TeamName")
                        .IsUnique();

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("SN_BNB.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("UserRole")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SN_BNB.Models.AssignedMatchPlayer", b =>
                {
                    b.HasOne("SN_BNB.Models.Match", "Match")
                        .WithMany("AssignedMatchPlayers")
                        .HasForeignKey("MatchID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SN_BNB.Models.Player", "Player")
                        .WithMany("AssignedMatchPlayers")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SN_BNB.Models.Fixture", b =>
                {
                    b.HasOne("SN_BNB.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationID");

                    b.HasOne("SN_BNB.Models.Season", "Season")
                        .WithMany("Fixtures")
                        .HasForeignKey("Season_idSeason")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SN_BNB.Models.Team", "AwayTeam")
                        .WithMany("AwayFixtures")
                        .HasForeignKey("idAwayTeam")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SN_BNB.Models.Team", "HomeTeam")
                        .WithMany("HomeFixtures")
                        .HasForeignKey("idHomeTeam")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SN_BNB.Models.Match", b =>
                {
                    b.HasOne("SN_BNB.Models.Fixture", "Fixture")
                        .WithMany("Matches")
                        .HasForeignKey("FixtureID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SN_BNB.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerID");
                });

            modelBuilder.Entity("SN_BNB.Models.Player", b =>
                {
                    b.HasOne("SN_BNB.Models.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SN_BNB.Models.Season_has_Team", b =>
                {
                    b.HasOne("SN_BNB.Models.Season", "Season")
                        .WithMany("Teams")
                        .HasForeignKey("SeasonID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SN_BNB.Models.Team", "Team")
                        .WithMany("Season_has_Teams")
                        .HasForeignKey("TeamID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SN_BNB.Models.Team", b =>
                {
                    b.HasOne("SN_BNB.Models.Division", "Division")
                        .WithMany("Teams")
                        .HasForeignKey("DivisionID")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
