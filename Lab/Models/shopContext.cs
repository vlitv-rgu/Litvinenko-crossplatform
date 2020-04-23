using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lab.Models
{
    public class shopContext : DbContext
    {
        public shopContext(DbContextOptions<shopContext> options)
            : base(options)
        {
        }

        public DbSet<shop> Shops { get; set; }
    }
}