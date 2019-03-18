using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Display(Name = "Season Title")]
        public string Season_Title { get; set; }
        [Required]
        [Display(Name = "Season Start Date")]
        public DateTime SeasonStart { get; set; }

        [Display(Name = "Season End Date")]
        public DateTime SeasonEnd { get; set; }

        [NotMappedAttribute]
        public Byte[] ExcelFile { get; set; }
        public ICollection<Fixture> Fixtures { get; set; }
        public ICollection<Season_has_Team> Teams { get; set; }
    }
}
