using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Season
    {
        public Season()
        {
            this.Fixtures = new HashSet<Fixture>();
        }
        public int ID { get; set; }
        [Required]
        [Display(Name = "Season Start Date")]
        public DateTime SeasonStart { get; set; }
        [Required]
        [Display(Name = "Season End Date")]
        public DateTime SeasonEnd { get; set; }

        public virtual ICollection<Fixture> Fixtures { get; set; }
    }
}
