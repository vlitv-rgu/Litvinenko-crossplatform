﻿using System;
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

        public IEnumerable<shop> getBigShops(IEnumerable<shop> shops)
        {
            return shops.Where(p => p.Goods.Count > 5);
        }

        public ICollection<good> GetSale(shop Shop)
        {
            var goods = Shop.Goods;
            foreach (var i in goods)
            {
                i.Price *= Convert.ToSingle(Shop.Sale()/100.0);
            }

            return goods;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<shop>()
               .OwnsMany(property => property.Goods);
        }
    }
}