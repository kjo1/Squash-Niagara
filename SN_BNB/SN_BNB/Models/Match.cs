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
        public Match()
        {
            AssignedMatchPlayers = new HashSet<AssignedMatchPlayer>();
        }
        [Required(ErrorMessage ="You must enter a score for player 1")]
        [Range(0, 6, ErrorMessage = "Please put point(s) between 0 and 6")]
        public int Player1Score { get; set; }

        [Range(0, 6, ErrorMessage = "Please put point(s) between 0 and 6")]
        [Required(ErrorMessage ="You must enter a score for player 2")]
        public int Player2Score { get; set; }

        [Required(ErrorMessage ="You must enter a match position")]
        [Range(1, 4)]
        public int MatchPosition { get; set; }

        [Required(ErrorMessage ="Please enter a date and time for this match")]
        public TimeSpan MatchTime { get; set; }


        [Display(Name = "Fixture")]
        public int FixtureID { get; set; }

        [Display(Name = "Players")]
        public int PlayerID { get; set; }
        


        public Fixture Fixture { get; set; }
        [Display(Name = "Players")]

        public Player Player { get; set; }

        public ICollection<AssignedMatchPlayer> AssignedMatchPlayers { get; set; }
    }
}
