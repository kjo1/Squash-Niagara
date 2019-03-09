using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Season_has_Team
    {
        public int TeamID { get; set; }
        public Team Team { get; set; }

        public int SeasonID { get; set; }
        public Season Season { get; set; }

    }
}
