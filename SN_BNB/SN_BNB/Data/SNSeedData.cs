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
                         DivisionID = 1

                     },
                    new Team
                    {
                        TeamName = "Ridley 1",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 2
                    },
                    new Team
                    {
                        TeamName = "BAC 1",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 3

                    },
                    new Team
                    {
                        TeamName = "Brock 1",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today,
                        DivisionID = 4

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
                     }
                     );
                    context.SaveChanges();
                }
            }
        }
    }
}

//if (!context.Teams.Any())
//{
//    context.Teams.AddRange(
//    new Team
//    {
//        TeamName = "White Oaks 1",
//        TeamPoints = 0,
//        TeamCreatedOn = DateTime.Today

//    },
//    new Team
//    {
//        TeamName = "Ridley 1",
//        TeamPoints = 0,
//        TeamCreatedOn = DateTime.Today
//    },
//    new Team
//    {
//        TeamName = "BAC 1",
//        TeamPoints = 0,
//        TeamCreatedOn = DateTime.Today

//    },
//    new Team
//    {
//        TeamName = "Brock 1",
//        TeamPoints = 0,
//        TeamCreatedOn = DateTime.Today
//    }
//    );
//    context.SaveChanges();
//}