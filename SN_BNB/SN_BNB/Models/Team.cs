using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Team
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }

        [Required]
        [Display(Name = "Team Points")]
        public int TeamPoints { get; set; }

        public DateTime? TeamCreatedOn { get; set; }

        public int DivisionID { get; set; }

        public virtual Division Division { get; set; }

        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<Fixture_has_Team> Fixture_has_Teams { get; set; }
    }
}
