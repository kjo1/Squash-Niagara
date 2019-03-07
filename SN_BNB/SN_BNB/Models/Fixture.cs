using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Fixture
    {
        public Fixture()
        {
            this.Matches = new HashSet<Match>();
            this.TeamScores = new HashSet<TeamScore>();
        }
        public int ID { get; set; }
        [Required]
        [Display(Name ="Date/Time")]
        public DateTime FixtureDatetime { get; set; }
        [Required]
        [Display(Name = "Home Team Score")]
        public int HomeScore { get; set; }
        [Required]
        [Display(Name = "Away Team Score")]
        public int AwayScore { get; set; }

        [Display(Name="Home Team")]
        public int idHomeTeam { get; set; }
        [Display(Name = "Away Team")]
        public int idAwayTeam { get; set; }
        [Display(Name = "Season")]
        public int Season_idSeason { get; set; }
       
        public string FixtureLocationCity { get; set; }

        public string FixtureLocationAddress { get; set; }

        public virtual Season Season { get; set; }

        public  ICollection<Match> Matches { get; set; }
        public  ICollection<TeamScore> TeamScores { get; set; }
    }
}
