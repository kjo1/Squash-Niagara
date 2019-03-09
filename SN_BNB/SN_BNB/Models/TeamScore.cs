using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class TeamScore
    {

        public int ID { get; set; }

        [Display(Name = "Fixture Score")]
        public string FixtureScore { get; set; }

        [Display(Name = "Approved")]
        public int TeamScoreApprovedBy { get; set; }


        public int TeamID { get; set; }
        public virtual Team Team { get; set; }

        public int FixtureID { get; set; }
        public virtual Fixture Fixture { get; set; }

    }
}
