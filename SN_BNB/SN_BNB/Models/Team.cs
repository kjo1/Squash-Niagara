﻿using System;
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
            Players = new HashSet<Player>();
            Season_has_Teams = new HashSet<Season_has_Team>();

        }

        public int ID { get; set; }
        [Required(ErrorMessage ="Please Select a Team")]
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }

        [Required]
        [Display(Name = "Team Points")]
        public int TeamPoints { get; set; }

        [Display(Name="Team Created")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? TeamCreatedOn { get; set; }

        [Display(Name = "Win(s)")]
        public int TeamWins { get; set; }

        [Display(Name = "Loss(es)")]
        public int TeamLosses { get; set; }

        [Display(Name="Division")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select the principal instrument the musician plays.")]
        public int DivisionID { get; set; }
        public virtual Division Division { get; set; }

        public ICollection<Player> Players { get; set; }
        
        [Display(Name="Seasons")]
        public ICollection<Season_has_Team> Season_has_Teams { get; set; }
    }
}
