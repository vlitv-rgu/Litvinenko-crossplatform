﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab.Models
{
    public class shop
    { 
        public long Id { get; set; }
        public string Name { get; set; }
        public List<good> Goods { get; set; }

        public shop()
        {
            Goods = new List<good>();
        }

        public float Sale()
        {
            Random rnd = new Random();
            return rnd.Next(99);
        }
        
    }
}
