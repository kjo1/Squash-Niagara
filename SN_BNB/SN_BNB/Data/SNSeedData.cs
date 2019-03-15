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
                        TeamName = "Team 1",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 2

                    },
                    new Team
                    {
                        TeamName = "Team 2",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 2

                    },
                    new Team
                    {
                        TeamName = "Team 3",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 3

                    },
                    new Team
                    {
                        TeamName = "Team 4",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 1

                    },
                    new Team
                    {
                        TeamName = "Team 5",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 3

                    }
                );
                    context.SaveChanges();
                }
                if (!context.News.Any())
                {
                    context.News.Add(
                     new News
                     {
                         Title = "Ridley 2 - the Champion",
                         Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vel eros donec ac odio. Tellus pellentesque eu tincidunt tortor aliquam nulla facilisi cras fermentum. Ut placerat orci nulla pellentesque dignissim enim sit amet. Massa eget egestas purus viverra accumsan in nisl nisi scelerisque. Tempor orci dapibus ultrices in iaculis nunc. In pellentesque massa placerat duis ultricies lacus sed turpis tincidunt. Pellentesque sit amet porttitor eget dolor. Non consectetur a erat nam at. Ut enim blandit volutpat maecenas. Enim nulla aliquet porttitor lacus luctus accumsan tortor posuere ac. Pellentesque habitant morbi tristique senectus et netus et malesuada. Ac turpis egestas integer eget. Facilisi etiam dignissim diam quis enim lobortis scelerisque fermentum.ra.Diam et.Amet nisl purus in molli.Cursus vitae congue mauris rhoncus aenean.Sit amet purus gravida quis blandit turpis cursus.Malesuada proin libero nunc consequat interdum varius sit.Egestas erat imperdiet sed euismod nisi porta lorem mollis aliquam.Vulputate ut pharetra sit amet.Dolor sit amet consectetur adipiscing elit pellentesque habitant morbi tristique.At consectetur lorem donec massa sapien faucibus et molestie.Nulla malesuada pellentesque elit eget.Id leo in vitae turpis.Massa placerat duis ultricies lacus.",
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
                            TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Brock 1").ID
                        },
                        new Player
                        {
                            FirstName = "Dave",
                            LastName = "Forgeron",
                            Gender = "M",
                            Email = "Dave_Forgeron@outlook.com",
                            Phone = 8880008000,
                            Position = 2,
                            TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "BAC 1").ID
                        },
                        new Player
                        {
                            FirstName = "Rachael",
                            LastName = "Forgeron",
                            Gender = "F",
                            Email = "Rachael_Forgeron@outlook.com",
                            Phone = 9055551202,
                            Position = 3,
                            TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Ridley 1").ID
                        },
                        new Player
                        {
                            FirstName = "Jakub",
                            LastName = "Lipinski",
                            Gender = "M",
                            Email = "Jakub_Lipinski@outlook.com",
                            Phone = 8880088000,
                            Position = 3,
                            TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "White Oaks 1").ID
                        },
                        new Player
                        {
                            FirstName = "Brendan",
                            LastName = "Yuki",
                            Gender = "M",
                            Email = "byuki@outlook.com",
                            Phone = 9056254141,
                            Position = 2,
                            TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Team 1").ID
                        },
                     new Player
                     {
                         FirstName = "Steven",
                         LastName = "Stone",
                         Gender = "M",
                         Email = "sstone@gmail.com",
                         Phone = 9053295625,
                         Position = 4,
                         TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Team 2").ID
                     },
                     new Player
                     {
                         FirstName = "Cynthia",
                         LastName = "Shirona",
                         Gender = "F",
                         Email = "cshirona@outlook.com",
                         Phone = 9055551257,
                         Position = 3,
                         TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Team 3").ID
                     },
                     new Player
                     {
                         FirstName = "May",
                         LastName = "Haruka",
                         Gender = "F",
                         Email = "mharuka@outlook.com",
                         Phone = 9055559892,
                         Position = 2,
                         TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Team 4").ID
                     },
                     new Player
                     {
                         FirstName = "Vanya",
                         LastName = "Hargreeves",
                         Gender = "F",
                         Email = "vhargreeves@outlook.com",
                         Phone = 9052251202,
                         Position = 1,
                         TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Team 5").ID
                     },
                     new Player
                     {
                         FirstName = "Alexander",
                         LastName = "Macedon",
                         Gender = "M",
                         Email = "greatalex@outlook.com",
                         Phone = 9053211202,
                         Position = 4,
                         TeamID = context.Teams.FirstOrDefault(t => t.TeamName == "Team 1").ID
                     }
                        );
                }
                if (!context.Seasons.Any())
                {
                    context.Seasons.AddRange(
                     new Season
                     {
                         Season_Title = "First",
                         SeasonStart = DateTime.Today,
                         SeasonEnd = DateTime.Today
                     }
                     );
                    context.SaveChanges();
                }

            }
        }
    }
}