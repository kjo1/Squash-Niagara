using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Team
    {
        public Team()
        {
            Players = new HashSet<Player>();
            Season_has_Teams = new HashSet<Season_has_Team>();
        }

        public int ID { get; set; }
        [Required(ErrorMessage = "Please Select an unique team name")]
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }

        [Display(Name = "Team Points")]
        [Range(0, 12)]
        public int TeamPoints { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Team Created")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? TeamCreatedOn { get; set; }

        [Display(Name = "Win(s)")]
        public int TeamWins
        {
            get
            {
                return  (HomeFixtures?
                    .Count(f => f.HomeScore > f.AwayScore) ?? 0) + 
                    (AwayFixtures?
                    .Count(f => f.AwayScore > f.HomeScore)?? 0);
            }
        }

        [Display(Name = "Loss(es)")]
        public int TeamLosses
        {
            get
            {
                return (HomeFixtures?
                    .Count(f => f.HomeScore < f.AwayScore) ?? 0) +
                    (AwayFixtures?
                    .Count(f => f.AwayScore < f.HomeScore) ?? 0);
            } 
        }

        [Display(Name = "Bio")]
        public string TeamBio { get; set; }

        [Display(Name = "Division")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select division of the team.")]
        public int DivisionID { get; set; }
        public virtual Division Division { get; set; }

        public ICollection<Player> Players { get; set; }
        public ICollection<Fixture> HomeFixtures { get; set; }
        public ICollection<Fixture> AwayFixtures { get; set; }

        public decimal WinPercent
        {
            get
            {
                int matches = HomeFixtures.Count + AwayFixtures.Count;
                return (decimal)TeamWins / (decimal)matches;
            }
        }

        [Display(Name = "Seasons")]
        public ICollection<Season_has_Team> Season_has_Teams { get; set; }
    }


}
