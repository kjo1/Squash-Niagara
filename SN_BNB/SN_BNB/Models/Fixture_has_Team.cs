using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class Fixture_has_Team
    {
        public int ID { get; set; }
        public int FixtureID { get; set; }
        public int TeamID { get; set; }

        public virtual Team Team { get; set; }
        public virtual Fixture Fixture { get; set; }
    }
}
