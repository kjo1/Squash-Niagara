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
        [Display(Name = "Name")]
        public string LocationName { get; set; }

        [Required]
        [Display(Name = "City")]
        public string LocationCity { get; set; }

        [Required]
        [Display(Name = "Street")]
        public string LocationStreet { get; set; }

        [Required]
        [Display(Name = "Building Number")]
        public int LocationBuildingNumber { get; set; }
    }
}
