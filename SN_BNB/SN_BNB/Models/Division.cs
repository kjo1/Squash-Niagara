using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Division
    {
        public Division()
        {
            this.Teams = new HashSet<Team>();
        }
        public int ID { get; set; }
        [Required]
        [Display(Name = "Division")]
        public string DivisionName { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
