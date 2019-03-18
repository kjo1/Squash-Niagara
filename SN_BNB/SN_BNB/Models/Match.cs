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
        public int Player1Score { get; set; }
        public int Player2Score { get; set; }
        [Required(ErrorMessage = "You must enter a match position")]
        [Range(1, 4)]
        public int MatchPositionPlayer1 { get; set; }
        [Required(ErrorMessage = "You must enter a match position")]
        [Range(1, 4)]
        public int MatchPositionPlayer2 { get; set; }
        [Required(ErrorMessage ="Please enter a date and time for this match")]
        public TimeSpan MatchTime { get; set; }


        [Display(Name = "Fixture")]
        public int FixtureID { get; set; }
        [Display(Name = "Player1")]
        public int Player1ID { get; set; }
        [Display(Name = "Player2")]
        public int Player2ID { get; set; }



        public Fixture Fixture { get; set; }
        [Display(Name = "Player1")]
        public Player Player1 { get; set; }
        [Display(Name = "Player2")]
        public Player Player2 { get; set; }

        public ICollection<AssignedMatchPlayer> AssignedMatchPlayers { get; set; }
    }
}
