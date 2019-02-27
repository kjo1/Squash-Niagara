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
            this.Players = new HashSet<Player>();
            this.Fixture_has_Teams = new HashSet<Fixture_has_Team>();
        }
        public int ID { get; set; }
        [Required]
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }

        [Required]
        [Display(Name = "Team Points")]
        public int TeamPoints { get; set; }

        [Display(Name="Team Created")]
        public DateTime? TeamCreatedOn { get; set; }

        [Display(Name="Division")]
        public int DivisionID { get; set; }

        public virtual Division Division { get; set; }

        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<Fixture_has_Team> Fixture_has_Teams { get; set; }
    }
}
