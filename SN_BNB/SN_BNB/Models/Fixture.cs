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
  
        [Display(Name ="Date/Time")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FixtureDateTime { get; set; }

        [Display(Name = "Home Team Score")]
        [Range(0, 5, ErrorMessage = "Please put point(s) between 0 and 5")]
        public int HomeScore { get; set; }

       
        [Display(Name = "Away Team Score")]
        [Range(0, 5, ErrorMessage = "Please put point(s) between 0 and 5")]
        public int AwayScore { get; set; }


        [Display(Name="Home Team")]
        [Required(ErrorMessage = "Please select a Home Team")]
        [Range(1, int.MaxValue)]
        public int idHomeTeam { get; set; }

        [Display(Name = "Away Team")]
        [Required(ErrorMessage = "Please select an Away Team")]
        [Range(1, int.MaxValue)]
        public int idAwayTeam { get; set; }

        [Display(Name = "Season")]
        [Required(ErrorMessage = "Please select a Season")]
        [Range(1, int.MaxValue)]
        public int Season_idSeason { get; set; }

        public int location_locationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Season Season { get; set; }
        public virtual Team HomeTeam { get; set; }
        public virtual Team AwayTeam { get; set; }

        public  ICollection<Match> Matches { get; set; }
    }
}
