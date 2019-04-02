using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB
{
    [NotMappedAttribute]
    public class MatchPosition
    {
        public MatchPosition(int ID, int POS)
        {
            this.position = POS;
            this.matchID = ID;
        }
        public int matchID;
        public int position;
    }
}
