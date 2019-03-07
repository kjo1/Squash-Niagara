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

        [Display(Name="Team Created")]
        public DateTime? TeamCreatedOn { get; set; }

        [Display(Name="Division")]
        public int DivisionID { get; set; }

        public virtual Division Division { get; set; }

        public ICollection<Player> Players { get; set; }
        
        [Display(Name="Seasons")]
        public ICollection<Season_has_Team> Season_has_Teams { get; set; }

        [Display(Name = "Fixtures")]
        public ICollection<Fixture_has_Team> Fixture_has_Teams { get; set; }
    }
}
