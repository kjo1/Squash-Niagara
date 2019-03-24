using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SN_BNB.ViewModels
{
    public class AssignedTeamVM
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public bool Assigned { get; set; }
    }
}
