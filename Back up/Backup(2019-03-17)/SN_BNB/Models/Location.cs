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
        public string LocationName { get; set; }
        [Required]
        public string LocationCity { get; set; }
        [Required]
        public string LocationStreet { get; set; }
        [Required]
        public int LocationBuildingNumber { get; set; }
    }
}
