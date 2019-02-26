using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Location
    {
        public int ID { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }

        public virtual Fixture Fixture { get; set; }
    }
}
