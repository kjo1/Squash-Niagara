using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class TeamCaptain
    {
        public int ID { get; set; }
        public int PlayerID { get; set; }
        public int TeamID { get; set; }

        public virtual Player Player { get; set; }
        public virtual Team Team { get; set; }
    }
}
