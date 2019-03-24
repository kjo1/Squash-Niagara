using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class AssignedMatchPlayer
    {
        public int PlayerID { get; set; }
        public Player Player { get; set; }

        public int MatchID { get; set; }
        public Match Match { get; set; }
    }
}
