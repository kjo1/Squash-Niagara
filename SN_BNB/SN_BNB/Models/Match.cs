using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Match
    {
        public int ID { get; set; }

        [Display(Name ="Score")]
        [Required(ErrorMessage = "You must enter a score for player 1")]
        [Range(0, 6, ErrorMessage = "Please put point(s) between 0 and 6")]
        public int Player1Score { get; set; }

        [Display(Name = "Score")]
        [Range(0, 6, ErrorMessage = "Please put point(s) between 0 and 6")]
        [Required(ErrorMessage = "You must enter a score for player 2")]
        public int Player2Score { get; set; }

        [Required(ErrorMessage = "You must enter a match position")]
        [Range(1, 4)]
        [Display(Name = "Position")]
        public int MatchPosition { get; set; }

        [Display(Name = "Time")]
        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan MatchTime { get; set; }

        [Display(Name = "Fixture")]
        public int FixtureID { get; set; }

        public Fixture Fixture { get; set; }

        [Display(Name = "Home Player")]
        [Required(ErrorMessage = "Please select a Home Player")]
        [Range(1, int.MaxValue)]
        public int Player1ID { get; set; }

        
        public virtual Player Player1 { get; set; }

        [Display(Name = "Away Player")]
        [Required(ErrorMessage = "Please select an Away Player")]
        [Range(1, int.MaxValue)]
        public int Player2ID { get; set; }


        public virtual Player Player2 { get; set; }

    }
}
