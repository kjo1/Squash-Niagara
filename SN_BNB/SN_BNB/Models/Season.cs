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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SeasonStart { get
            {
                DateTime earliest = DateTime.MaxValue;
                foreach(Fixture fixture in Fixtures)
                {
                    earliest = new DateTime(Math.Min(earliest.Ticks, fixture.FixtureDateTime.Ticks));
                }
                return earliest;
            }
        }

        [Display(Name = "Season End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SeasonEnd
        {
            get
            {
                DateTime latest = DateTime.MinValue;
                foreach (Fixture fixture in Fixtures)
                {
                    latest = new DateTime(Math.Max(latest.Ticks, fixture.FixtureDateTime.Ticks));
                }
                return latest;
            }
        }

        [NotMappedAttribute]
        public Byte[] ExcelFile { get; set; }
        public ICollection<Fixture> Fixtures { get; set; }
        public ICollection<Season_has_Team> Teams { get; set; }
    }
}
