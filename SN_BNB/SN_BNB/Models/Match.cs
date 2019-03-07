using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Match : IValidatableObject
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="You must enter a score for player 1")]
        public int Player1Score { get; set; }

        [Required(ErrorMessage ="You must enter a score for player 2")]
        public int Player2Score { get; set; }
        [Required(ErrorMessage ="You must enter a match position")]
        [Range(1, 4)]
        public int MatchPosition { get; set; }
        [Required(ErrorMessage ="Please enter a date and time for this match")]
        public DateTime MatchDateTime { get; set; }


        [Display(Name = "Fixture")]
        public int FixtureID { get; set; }
        [Display(Name = "Player 1")]
        public int Player1ID { get; set; }
        [Display(Name = "Player 2")]
        public int Player2ID { get; set; }
        

        public Fixture Fixture { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(MatchDateTime < DateTime.Today)
            {
                yield return new ValidationResult("Date of match cannot be in the past");
            }
        }
    }
}
