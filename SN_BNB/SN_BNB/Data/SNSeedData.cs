using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SN_BNB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SN_BNB.Data
{
    public class SNSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            using (var context = new SNContext(
                serviceProvider.GetRequiredService<DbContextOptions<SNContext>>()))
            {
                // Look for any Patients.  Since we can't have patients without Doctors.
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
                         TeamCreatedOn = DateTime.Today

                     },
                    new Team
                    {
                        TeamName = "Ridley 1",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today
                    },
                    new Team
                    {
                        TeamName = "BAC 1",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today

                    },
                    new Team
                    {
                        TeamName = "Brock 1",
                        TeamPoints = 0,
                        TeamCreatedOn = DateTime.Today
                    }
                );
                    context.SaveChanges();
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
                    if (!context.Players.Any())
                    {
                        context.Players.AddRange(
                         new Player
                         {
                             FirstName = "Matt",
                             LastName = "Bowie",
                             Gender = "M",
                             Email = "Matt_Bowie@outlook.com",
                             Phone = Convert.ToInt32(8880008800),
                             Position = 1,
                             TeamID = 1
                         },

                         new Player
                         {
                             FirstName = "Dave",
                             LastName = "Forgeron",
                             Gender = "M",
                             Email = "Dave_Forgeron@outlook.com",
                             Phone = Convert.ToInt32(8880008800),
                             Position = 2,
                             TeamID = 2
                         },
                         new Player
                         {
                             FirstName = "Rachael",
                             LastName = "Forgeron",
                             Gender = "F",
                             Email = "Rachael_Forgeron@outlook.com",
                             Phone = Convert.ToInt32(8880008800),
                             Position = 3,
                             TeamID = 3
                         },
                         new Player
                         {
                             FirstName = "Jakub",
                             LastName = "Lipinski",
                             Gender = "M",
                             Email = "Jakub_Lipinski@outlook.com",
                             Phone = Convert.ToInt32(8880008800),
                             Position = 3,
                             TeamID = 3
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
