using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Fixture : IValidatableObject
    {
        public Fixture()
        {
            this.Matches = new HashSet<Match>();
        }
        public int ID { get; set; }

        //[Required]
        public string Title
        {
            get {
                    return HomeTeam.TeamName + " vs " + AwayTeam.TeamName;
                }
        }


        [Display(Name = "Date")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dddd, MMMM dd, yyy}", ApplyFormatInEditMode = true)]
        public DateTime FixtureDateTime { get; set; }

        [Display(Name = "Home Team Score")]

        public int HomeScore
        {
            get
            {
                return (Matches?.Count(m => m.Player1Score > m.Player2Score) ?? 0);
            }
        }


        [Display(Name = "Away Team Score")]
        public int AwayScore
        {
            get
            {
                return Matches?.Count(m => m.Player2Score > m.Player1Score) ?? 0;
            }
        }


        [Display(Name = "Home Team")]
        [Required(ErrorMessage = "Please select a Home Team")]
        [Range(1, int.MaxValue)]
        public int idHomeTeam { get; set; }

        [Display(Name = "Away Team")]
        [Required(ErrorMessage = "Please select an Away Team")]
        [Range(1, int.MaxValue)]
        public int idAwayTeam { get; set; }

        [Display(Name = "Bonus Point")]
        public string BonusPoint { get; set; }

        [Display(Name = "Season")]
        [Required(ErrorMessage = "Please select a Season")]
        [Range(1, int.MaxValue)]
        public int Season_idSeason { get; set; }

        public int location_locationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Season Season { get; set; }
        public virtual Team HomeTeam { get; set; }
        public virtual Team AwayTeam { get; set; }

        public ICollection<Match> Matches { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            switch (this.BonusPoint)
            {
                case "Home":         //award 1 point to home team
                    break;
                case "Away":        //award 1 point to away team
                    break;
                case "Split":         //each team is awarded 0.5 points
                    break;
                case null:      //the bonus point is not yet assigned
                    break;
                default:
                    results.Add(new ValidationResult("Invalid entry for Bonus Point"));
                    break;
            }
            return results;
        }
    }
}
