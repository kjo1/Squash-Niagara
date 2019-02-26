using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.Models
{
    public class ApprovedBy
    {
        public int ID { get; set; }
        public int TeamScoresID { get; set; }
        public int UserID { get; set; }

        public virtual TeamScore TeamScore { get; set; }
        public virtual User User { get; set; }
    }
}
