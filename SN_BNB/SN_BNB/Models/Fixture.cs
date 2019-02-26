﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Fixture
    {
        public Fixture()
        {
            this.Fixture_has_Teams = new HashSet<Fixture_has_Team>();
            this.Matches = new HashSet<Match>();
            this.TeamScores = new HashSet<TeamScore>();
        }
        public int ID { get; set; }
        [Required]
        [Display(Name ="Date/Time")]
        public DateTime Datetime { get; set; }
        [Required]
        [Display(Name = "Home Team Score")]
        public int HomeScore { get; set; }
        [Required]
        [Display(Name = "Away Team Score")]
        public int AwayScore { get; set; }

        [Display(Name="Home Team")]
        public int HomeTeamID { get; set; }
        [Display(Name = "Away Team")]
        public int AwayTeamID { get; set; }
        [Display(Name = "Season")]
        public int SeasonID { get; set; }
        [Display(Name = "Location")]
        public int LocationID { get; set; }

        public virtual Location Location { get; set; }
        public virtual Season Season { get; set; }

        public virtual ICollection<Fixture_has_Team> Fixture_has_Teams { get; set; }
        public virtual ICollection<Match> Matches { get; set; }
        public virtual ICollection<TeamScore> TeamScores { get; set; }
    }
}
