using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Fixture
    {
        public int ID { get; set; }
        [Required]
        [Display(Name ="Date/Time")]
        public DateTime Datetime { get; set; }
        [Required]
        [Display(Name = "Home Team Score")]
        public int HomeScore { get; set; }
        [Required]
        [Display(Name = "Away Team Score")]
        public int AwayScore { get; set; }

        [Display(Name="Home Team")]
        public int HomeTeamID { get; set; }
        [Display(Name = "Away Team")]
        public int AwayTeamID { get; set; }
        [Display(Name = "Season")]
        public int SeasonID { get; set; }
        [Display(Name = "Location")]
        public int LocationID { get; set; }

        public virtual Location Location { get; set; }
        public virtual Season Season { get; set; }

        public  ICollection<Match> Matches { get; set; }
        public  ICollection<TeamScore> TeamScores { get; set; }
        public ICollection<Fixture_has_Team> Fixture_Has_Teams { get; set; }
    }
}
