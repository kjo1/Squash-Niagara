using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Player
    {
        public Player()
        {
            this.HomeMatches = new HashSet<Match>();
            this.AwayMatches = new HashSet<Match>();
        }
        public int ID { get; set; }

        [Display(Name = "Player")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You cannot leave the first name blank.")]
        [StringLength(45, ErrorMessage = "First name cannot be more than 45 characters long.")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(45, ErrorMessage = "Middle name cannot be more than 45 characters long.")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You cannot leave the last name blank.")]
        [StringLength(100, ErrorMessage = "Last name cannot be more than 100 characters long.")]
        public string LastName { get; set; }

        public string Gender { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number (no spaces).")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:(###) ###-####}", ApplyFormatInEditMode = false)]
        public Int64 Phone { get; set; }

        [Required(ErrorMessage = "Position is required.(1 to 4)")]
        [Range(1, 4)]
        public int Position { get; set; }

        public int Played { get; set; }

        [Display(Name = "Won")]
        public int Win { get; set; }

        [Display(Name = "Lost ")]
        public int Loss { get; set; }

        public int For { get; set; }

        public int Against { get; set; }

        public int Points { get; set; }

        [Required(ErrorMessage = "You must select a assigned Team")]
        [Display(Name = "Team")]
        public int TeamID { get; set; }

        [Required(ErrorMessage = "You must select if you want to hide your phone number from other players.")]
        [Display(Name = "Hide your phone number from other players?")]
        public bool PlayerWantsInfoHidden { get; set; }

        public virtual Team Team { get; set; }

        public virtual ICollection<Match> HomeMatches { get; set; }
        public virtual ICollection<Match> AwayMatches { get; set; }

        [NotMappedAttribute]
        public Byte[] ExcelFile { get; set; }

        // Number of matches played at a particular position
        public decimal MatchesByPosition(int matchPosition)
        {
            int playedInPosition = 0;
            if (Played == 0)
                return 0m;
            else
            {
                if (HomeMatches != null) playedInPosition += HomeMatches.Where(m => m.Player1ID == ID && m.MatchPosition == matchPosition).Count();
                if (AwayMatches != null) playedInPosition += AwayMatches.Where(m => m.Player2ID == ID && m.MatchPosition == matchPosition).Count();
                return Convert.ToDecimal(playedInPosition) / Convert.ToDecimal(Played);
            }
        }


        public decimal WinPerPos(int matchPosition)
        {
            int playedInPosition = 0;
            int wonInPosition = 0;
            List<Match> homeMatchesInPosition = HomeMatches.Where(m => m.Player1ID == ID && m.MatchPosition == matchPosition).ToList<Match>();
            List<Match> awayMatchesInPosition = AwayMatches.Where(m => m.Player2ID == ID && m.MatchPosition == matchPosition).ToList<Match>();
            if (Played==0)
                return 0m;
            else
            {
                if (HomeMatches != null)
                {
                    playedInPosition += homeMatchesInPosition.Count;
                    foreach(Match match in HomeMatches)
                    {
                        if (match.Player1Score > match.Player2Score) wonInPosition += 1;
                    }
                }
                if (AwayMatches != null)
                {
                    playedInPosition += awayMatchesInPosition.Count;
                    foreach (Match match in AwayMatches)
                    {
                        if (match.Player1Score < match.Player2Score) wonInPosition += 1;
                    }
                }
                return (Convert.ToDecimal(wonInPosition) / Convert.ToDecimal(playedInPosition));
            }
        }
    }
}
