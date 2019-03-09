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
        public SNContext(DbContextOptions<SNContext> options)
            : base(options)
        {
        }

        public DbSet<Division> Divisions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<Season_has_Team> SeasonTeams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<TeamScore> TeamScores { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("SN");

            //Cascade Deletes
            modelBuilder.Entity<Division>()
                .HasMany<Team>(d => d.Teams)
                .WithOne(t => t.Division)
                .HasForeignKey(t => t.DivisionID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasMany<Player>(d => d.Players)
                .WithOne(t => t.Team)
                .HasForeignKey(t => t.TeamID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Season>()
                .HasMany<Fixture>(d => d.Fixtures)
                .WithOne(t => t.Season)
                .HasForeignKey(t => t.Season_idSeason)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Season_has_Team>()
                .HasOne(t => t.Team)
                .WithMany(st => st.Season_has_Teams)
                .HasForeignKey(t => t.TeamID)
                .OnDelete(DeleteBehavior.Restrict);

            //Add a unique index to the Email Address
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Email)
                .IsUnique();
            

            //Add this so you don't get Cascade Delete
            modelBuilder.Entity<player_team>()
                .HasOne(pt => pt.Match)
                .WithMany(t => t.player_team)
                .HasForeignKey(pt => pt.MatchID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeamScore>()
                .HasOne(ts => ts.Fixture)
                .WithMany(f => f.TeamScores)
                .HasForeignKey(ts => ts.FixtureID)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique Fields
            modelBuilder.Entity<Division>()
                .HasIndex(d => d.DivisionName)
                .IsUnique();

            modelBuilder.Entity<Team>()
                .HasIndex(d => d.TeamName)
                .IsUnique();

            //Add a unique index to the Email Address
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Email)
                .IsUnique();

            // Insection Table Key Declarations
            modelBuilder.Entity<Season_has_Team>()
                .HasKey(t => new { t.TeamID, t.SeasonID });

            modelBuilder.Entity<TeamScore>()
                .HasKey(t => new { t.TeamID, t.FixtureID });

            modelBuilder.Entity<player_team>()
            .HasKey(t => new { t.MatchID, t.PlayerID });
        }

        public DbSet<SN_BNB.Models.News> News { get; set; }

    }
}

//modelBuilder.Entity<Player>()
//    .HasMany<Match>(p => p.Matches)
//    .WithOne(m => m.)
//    .HasForeignKey(m => m.PlayerID)
//    .OnDelete(DeleteBehavior.Restrict);
