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
        }
        public int ID { get; set; }
        [Required]
        [Display(Name ="Date/Time")]
        public DateTime FixtureDateTime { get; set; }

        [Required]
        [Display(Name = "Home Team Score")]
        public int HomeScore { get; set; }

        [Required]
        [Display(Name = "Away Team Score")]
        public int AwayScore { get; set; }

        [Display(Name="Home Team")]
        [Range(1, int.MaxValue)]
        public int idHomeTeam { get; set; }

        [Display(Name = "Away Team")]
        [Range(1, int.MaxValue)]
        public int idAwayTeam { get; set; }

        [Display(Name = "Season")]
        [Range(1, int.MaxValue)]
        public int Season_idSeason { get; set; }

        [Required]
        public int location_locationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Season Season { get; set; }
        public virtual Team HomeTeam { get; set; }
        public virtual Team AwayTeam { get; set; }

        public  ICollection<Match> Matches { get; set; }
    }
}
