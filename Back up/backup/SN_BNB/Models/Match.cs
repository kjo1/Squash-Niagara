using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Match
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Date/Time")]
        public DateTime Datetime { get; set; }

        [Display(Name = "Fixture")]
        public int FixtureID { get; set; }

        public Fixture Fixture { get; set; }
    }
}
