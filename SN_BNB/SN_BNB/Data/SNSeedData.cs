using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SN_BNB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SN_BNB.Data
{
    public static class SNSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            using (var context = new SNContext(
                serviceProvider.GetRequiredService<DbContextOptions<SNContext>>()))
            {
                System.Diagnostics.Debug.WriteLine("SEEDING THE DATABASE...");
                if (!context.Divisions.Any())
                {
                    context.Divisions.AddRange(
                     new Division
                     {
                         DivisionName = "2018-2019 Men's Division 1/2"
                     },
                     new Division
                     {
                         DivisionName = "2018-2019 Men's Division 3"
                     },
                     new Division
                     {
                         DivisionName = "2018-2019 Men's Division 4"
                     },
                     new Division
                     {
                         DivisionName = "2018-2019 Women's Division"
                     }
                );
                    context.SaveChanges();
                }
                if (!context.Teams.Any())
                {
                    context.Teams.AddRange(
                     new Team
                     {
                         TeamName = "White Oaks 1",
                         TeamPoints = 0,
                         TeamCreatedOn = DateTime.Today,
                         DivisionID = 1,
                         TeamBio = "This is Team White Oaks 1"
                     },
                    new Team
                    {
                        TeamName = "Ridley 1",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 2,
                        TeamBio = "This is Team Ridley 1"
                    },
                    new Team
                    {
                        TeamName = "BAC 1",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 3,
                        TeamBio = "This is Team BAC 1"

                    },
                    new Team
                    {
                        TeamName = "Brock 1",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 4,
                        TeamBio = "This is Team Brock 1"
                    },
                    new Team
                    {
                        TeamName = "BAC 2",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 2

                    },
                    new Team
                    {
                        TeamName = "CAB 2",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 2

                    },
                    new Team
                    {
                        TeamName = "CAB 1",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 3

                    },
                    new Team
                    {
                        TeamName = "Brick 1",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 1

                    },
                    new Team
                    {
                        TeamName = "Brick 2",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 3

                    },
                    new Team
                    {
                        TeamName = "Brock 2",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 4

                    },
                    new Team
                    {
                        TeamName = "Ridley 2",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 3

                    },
                    new Team
                    {
                        TeamName = "White Oaks 2",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 2

                    },
                    new Team
                    {
                        TeamName = "Brock 3",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 1

                    },
                    new Team
                    {
                        TeamName = "Ridley 3",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 2

                    }
                );
                    context.SaveChanges();
                }
                if (!context.News.Any())
                {
                    context.News.AddRange(
                     new News
                     {
                         Title = "Ridley 2 - The Champion",
                         Content = "Ridley 2 dominated the competition earning the most points out of all teams by a large margin this season! Having two players outperforming the rest in their respective positions they swept through the league with ease.",
                         Date = DateTime.Now
                     },
                     new News
                     {
                         Title = "Matches Cancelled Today",
                         Content = "Due to the extreme weather conditions we are issuing a cancellation of todays matches. We will post when the matches will be rescheduled shortly. Sorry for the inconvenience.",
                         Date = DateTime.Now
                     },
                     new News
                     {
                         Title = "Registration for next season available now!",
                         Content = "The registration for next season is now available. The final date to sign up for the season is April 20th, no late registrations will be taken. Thank you",
                         Date = DateTime.Now
                     }
                );
                    context.SaveChanges();
                }
                if (!context.Players.Any())
                {
                    context.Players.AddRange(
                    new Player
                    {
                        FirstName = "Matt",
                        LastName = "Bowie",
                        Gender = "M",
                        Email = "Matt_Bowie@outlook.com",
                        Phone = 8880000800,
                        Position = 1,
                        TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Brock 1").ID,
                        PlayerWantsInfoHidden = false
                    },
                    new Player
                    {
                        FirstName = "Dave",
                        LastName = "Forgeron",
                        Gender = "M",
                        Email = "Dave_Forgeron@outlook.com",
                        Phone = 8880008000,
                        Position = 2,
                        TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "BAC 1").ID,
                        PlayerWantsInfoHidden = false
                    },
                    new Player
                    {
                        FirstName = "Rachael",
                        LastName = "Forgeron",
                        Gender = "F",
                        Email = "Rachael_Forgeron@outlook.com",
                        Phone = 9055551202,
                        Position = 3,
                        TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Ridley 1").ID,
                        PlayerWantsInfoHidden = true
                    },
                    new Player
                    {
                        FirstName = "Jakub",
                        LastName = "Lipinski",
                        Gender = "M",
                        Email = "Jakub_Lipinski@outlook.com",
                        Phone = 8880088000,
                        Position = 3,
                        TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "White Oaks 1").ID,
                        PlayerWantsInfoHidden = true
                    },
                    new Player
                    {
                        FirstName = "Brendan",
                        LastName = "Yuki",
                        Gender = "M",
                        Email = "byuki@outlook.com",
                        Phone = 9056254141,
                        Position = 2,
                        TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Brock 2").ID,
                        PlayerWantsInfoHidden = true
                    },
                     new Player
                     {
                         FirstName = "Steven",
                         LastName = "Stone",
                         Gender = "M",
                         Email = "sstone@gmail.com",
                         Phone = 9053295625,
                         Position = 4,
                         TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "CAB 2").ID,
                         PlayerWantsInfoHidden = true
                     },
                     new Player
                     {
                         FirstName = "Cynthia",
                         LastName = "Shirona",
                         Gender = "F",
                         Email = "cshirona@outlook.com",
                         Phone = 9055551257,
                         Position = 3,
                         TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "CAB 1").ID,
                         PlayerWantsInfoHidden = false
                     },
                     new Player
                     {
                         FirstName = "May",
                         LastName = "Haruka",
                         Gender = "F",
                         Email = "mharuka@outlook.com",
                         Phone = 9055559892,
                         Position = 2,
                         TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Brick 1").ID,
                         PlayerWantsInfoHidden = false
                     },
                     new Player
                     {
                         FirstName = "Vanya",
                         LastName = "Hargreeves",
                         Gender = "F",
                         Email = "vhargreeves@outlook.com",
                         Phone = 9052251202,
                         Position = 1,
                         TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Brick 2").ID,
                         PlayerWantsInfoHidden = true
                     },
                     new Player
                     {
                         FirstName = "Alexander",
                         LastName = "Macedon",
                         Gender = "M",
                         Email = "greatalex@outlook.com",
                         Phone = 9053211202,
                         Position = 4,
                         TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Ridley 2").ID,
                         PlayerWantsInfoHidden = false
                     },
                     new Player
                     {
                         FirstName = "Archie",
                         LastName = "Price",
                         Gender = "M",
                         Email = "aprice@outlook.com",
                         Phone = 2892344321,
                         Position = 4,
                         TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "White Oaks 2").ID,
                         PlayerWantsInfoHidden = false
                     },
                     new Player
                     {
                         FirstName = "Maxie",
                         LastName = "Lang",
                         Gender = "M",
                         Email = "mlang@outlook.com",
                         Phone = 2890988907,
                         Position = 3,
                         TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Brock 3").ID,
                         PlayerWantsInfoHidden = true
                     },
                     new Player
                     {
                         FirstName = "Nate",
                         LastName = "River",
                         Gender = "M",
                         Email = "nriver@outlook.com",
                         Phone = 2896549874,
                         Position = 2,
                         TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Ridley 3").ID,
                         PlayerWantsInfoHidden = true
                     },
                     new Player
                     {
                         FirstName = "Serena",
                         LastName = "Paschall",
                         Gender = "F",
                         Email = "spaschall@outlook.com",
                         Phone = 2899518475,
                         Position = 1,
                         TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "White Oaks 1").ID,
                         PlayerWantsInfoHidden = false
                     },
                     new Player
                     {
                         FirstName = "Dawn",
                         LastName = "Jenness",
                         Gender = "F",
                         Email = "djenness@outlook.com",
                         Phone = 2896231592,
                         Position = 2,
                         TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Ridley 2").ID,
                         PlayerWantsInfoHidden = false
                     }
                        );
                    context.SaveChanges();
                }
                if (!context.Seasons.Any())
                {
                    context.Seasons.AddRange(
                     new Season
                     {
                         Season_Title = "First",
                         SeasonStart = DateTime.Today,
                         SeasonEnd = DateTime.Today
                     },
                     new Season
                     {
                         Season_Title = "Second",
                         SeasonStart = DateTime.Today,
                         SeasonEnd = DateTime.Today
                     },
                     new Season
                     {
                         Season_Title = "Third",
                         SeasonStart = DateTime.Today,
                         SeasonEnd = DateTime.Today
                     },
                     new Season
                     {
                         Season_Title = "Fourth",
                         SeasonStart = DateTime.Today,
                         SeasonEnd = DateTime.Today
                     }
                     );
                    context.SaveChanges();
                }
                if (!context.Location.Any())
                {
                    context.Location.AddRange(
                     new Location
                     {
                         LocationName = "Niagara College",
                         LocationCity = "Welland",
                         LocationStreet = "Merritt",
                         LocationBuildingNumber = 1                         
                     },
                     new Location
                     {
                         LocationName = "Brock University",
                         LocationCity = "Thorold",
                         LocationStreet = "St. Paul",
                         LocationBuildingNumber = 22
                     },
                     new Location
                     {
                         LocationName = "Ridley College",
                         LocationCity = "St. Catharines",
                         LocationStreet = "Fourth Ave.",
                         LocationBuildingNumber = 33
                     },
                     new Location
                     {
                         LocationName = "White Oaks",
                         LocationCity = "Niagara On The Lake",
                         LocationStreet = "Merritt",
                         LocationBuildingNumber = 44
                     }
                     );
                    context.SaveChanges();
                }
                if (!context.Fixtures.Any())
                {
                    context.Fixtures.AddRange(
                     new Fixture
                     {
                         FixtureDateTime = DateTime.Parse("2019-04-01"),
                         HomeScore = 0,
                         AwayScore = 0,
                         idHomeTeam = 1,
                         idAwayTeam = 2,
                         Season_idSeason = 1,
                         location_locationId = 1
                     },
                     new Fixture
                     {
                         FixtureDateTime = DateTime.Parse("2019-04-05"),
                         HomeScore = 0,
                         AwayScore = 0,
                         idHomeTeam = context.Teams.FirstOrDefault(t => t.TeamName == "Brock 1").ID,
                         idAwayTeam = context.Teams.FirstOrDefault(t => t.TeamName == "Ridley 1").ID,
                         Season_idSeason = context.Seasons.FirstOrDefault(s => s.Season_Title == "Second").ID
                     },
                     new Fixture
                     {
                         FixtureDateTime = DateTime.Parse("2019-04-08"),
                         HomeScore = 0,
                         AwayScore = 0,
                         idHomeTeam = context.Teams.FirstOrDefault(t => t.TeamName == "Brock 2").ID,
                         idAwayTeam = context.Teams.FirstOrDefault(t => t.TeamName == "BAC 1").ID,
                         Season_idSeason = context.Seasons.FirstOrDefault(s => s.Season_Title == "Third").ID
                     },
                     new Fixture
                     {
                         FixtureDateTime = DateTime.Parse("2019-04-21"),
                         HomeScore = 0,
                         AwayScore = 0,
                         idHomeTeam = context.Teams.FirstOrDefault(t => t.TeamName == "Brock 3").ID,
                         idAwayTeam = context.Teams.FirstOrDefault(t => t.TeamName == "Ridley 2").ID,
                         Season_idSeason = context.Seasons.FirstOrDefault(s => s.Season_Title == "First").ID
                     },
                     new Fixture
                     {
                         FixtureDateTime = DateTime.Parse("2019-04-16"),
                         HomeScore = 0,
                         AwayScore = 0,
                         idHomeTeam = context.Teams.FirstOrDefault(t => t.TeamName == "Brock 2").ID,
                         idAwayTeam = context.Teams.FirstOrDefault(t => t.TeamName == "BAC 2").ID,
                         Season_idSeason = context.Seasons.FirstOrDefault(s => s.Season_Title == "Second").ID
                     },
                     new Fixture
                     {
                         FixtureDateTime = DateTime.Parse("2019-04-18"),
                         HomeScore = 0,
                         AwayScore = 0,
                         idHomeTeam = context.Teams.FirstOrDefault(t => t.TeamName == "White Oaks 1").ID,
                         idAwayTeam = context.Teams.FirstOrDefault(t => t.TeamName == "BAC 1").ID,
                         Season_idSeason = context.Seasons.FirstOrDefault(s => s.Season_Title == "Fourth").ID
                     },
                     new Fixture
                     {
                         FixtureDateTime = DateTime.Parse("2019-04-21"),
                         HomeScore = 0,
                         AwayScore = 0,
                         idHomeTeam = context.Teams.FirstOrDefault(t => t.TeamName == "CAB 2").ID,
                         idAwayTeam = context.Teams.FirstOrDefault(t => t.TeamName == "BAC 3").ID,
                         Season_idSeason = context.Seasons.FirstOrDefault(s => s.Season_Title == "Fourth").ID
                     },
                     new Fixture
                     {
                         FixtureDateTime = DateTime.Parse("2019-04-01"),
                         HomeScore = 0,
                         AwayScore = 0,
                         idHomeTeam = context.Teams.FirstOrDefault(t => t.TeamName == "CAB 1").ID,
                         idAwayTeam = context.Teams.FirstOrDefault(t => t.TeamName == "Ridley 2").ID,
                         Season_idSeason = context.Seasons.FirstOrDefault(s => s.Season_Title == "First").ID
                     }
                     );
                    context.SaveChanges();
                }

            }
        }
    }
}