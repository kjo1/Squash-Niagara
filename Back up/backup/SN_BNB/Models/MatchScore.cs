using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class MatchScore
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Match Scores")]
        public int MatchScoresPoints { get; set; }


        public int PlayerID { get; set; }

        public virtual Player Player { get; set; }
    }
}
