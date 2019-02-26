using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class User
    {
        public int ID { get; set; }
        [Required]
        [Display(Name="User Role")]
        public string UserRole { get; set; }

        public virtual ICollection<ApprovedBy> ApprovedBys { get; set; }
    }
}
