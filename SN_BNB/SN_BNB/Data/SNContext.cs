using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SN_BNB.Models;

namespace SN_BNB.Data
{
    public class SNContext : DbContext
    {
        public SNContext (DbContextOptions<SNContext> options)
            : base(options)
        {
        }

        public DbSet<Division> Divisions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<ApprovedBy> Approved { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<Fixture_has_Team> FixtureTeams { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchScore> MatchScores { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<TeamScore> TeamScores { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("SN");

            modelBuilder.Entity<Division>()
                .HasMany<Team>(d => d.Teams)
                .WithOne(t => t.Division)
                .HasForeignKey(t => t.DivisionID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasMany<Fixture_has_Team>(d => d.Fixture_has_Teams)
                .WithOne(t => t.Team)
                .HasForeignKey(t => t.TeamID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasMany<Player>(d => d.Players)
                .WithOne(t => t.Team)
                .HasForeignKey(t => t.TeamID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Season>()
                .HasMany<Fixture>(d => d.Fixtures)
                .WithOne(t => t.Season)
                .HasForeignKey(t => t.SeasonID)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Division>()
                .HasIndex(d => d.DivisionName)
                .IsUnique();

            modelBuilder.Entity<Team>()
                .HasIndex(d => d.TeamName)
                .IsUnique();

        }

    }
}
