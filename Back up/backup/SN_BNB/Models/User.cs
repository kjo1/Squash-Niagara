using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class User
    {
        public User()
        {
            this.ApprovedBys = new HashSet<ApprovedBy>();
        }
        public int ID { get; set; }
        [Required]
        [Display(Name="User Role")]
        public string UserRole { get; set; }

        [NotMappedAttribute]
        public Byte[] ExcelFile { get; set; }
        public ICollection<ApprovedBy> ApprovedBys { get; set; }
    }
}
