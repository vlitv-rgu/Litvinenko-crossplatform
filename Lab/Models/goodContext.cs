using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lab.Models
{ 
    public class goodContext : DbContext
    {
        public goodContext(DbContextOptions<goodContext> options)
            : base(options)
        {
        }

        public DbSet<good> Goods { get; set; }
    }
}