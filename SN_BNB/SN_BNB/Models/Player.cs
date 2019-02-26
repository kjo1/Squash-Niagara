using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Player
    {
        public Player()
        {
            this.MatchScores = new HashSet<MatchScore>();
        }
        public int ID { get; set; }
        [Required]
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int Phone { get; set; }
        [Required]
        public string Position { get; set; }

        public int TeamID { get; set; }

        public virtual Team Team { get; set; }

        public virtual ICollection<MatchScore> MatchScores { get; set; }


    }
}
