using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab.Models
{
    public class shop
    { 
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<good> Goods { get; set; }

        public int Sale()
        {
            Random rnd = new Random();
            return rnd.Next(99);
        }
        
    }
}
