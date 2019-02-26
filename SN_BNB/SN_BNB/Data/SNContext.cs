using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SN_BNB.Models;

namespace SN_BNB.Data
{
    public class SNContext : DbContext
    {
        public SNContext (DbContextOptions<SNContext> options)
            : base(options)
        {
        }

        public DbSet<SN_BNB.Models.Division> Division { get; set; }
    }
}
