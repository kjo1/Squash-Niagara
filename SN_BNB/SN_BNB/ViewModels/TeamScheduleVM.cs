using SN_BNB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.ViewModels
{
    public class TeamScheduleVM
    {
        public TeamScheduleVM(Fixture fixture)
        {
            HomeTeamName = fixture.HomeTeam.TeamName;
            AwayTeamName = fixture.AwayTeam.TeamName;
            Date = fixture.FixtureDateTime.ToShortDateString();
        }

        public string HomeTeamName;
        public string AwayTeamName;
        public string Date;

    }
}
