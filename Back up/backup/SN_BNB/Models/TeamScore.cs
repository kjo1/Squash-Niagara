using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class TeamScore
    {
        public TeamScore()
        {
            this.ApprovedBys = new HashSet<ApprovedBy>();
        }
        public int ID { get; set; }
        [Required]
        [Display(Name="Fixture Score")]
        public string FixtureScore { get; set; }

        public int TeamID { get; set; }
        public int FixtureID { get; set; }

        public virtual Team Team { get; set; }
        public virtual Fixture Fixture { get; set; }

        public ICollection<ApprovedBy> ApprovedBys { get; set; }
    }
}
