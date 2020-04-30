using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lab.Models
{
    public class mainContext : DbContext
    {
        public mainContext(DbContextOptions<mainContext> options)
            : base(options)
        {
        }

        public DbSet<good> Goods { get; set; }
        public DbSet<shop> Shops { get; set; }


        public IEnumerable<good> getCheapGoods(IEnumerable<good> goods)
        {
            return
                from good in goods
                where good.Price < 1000
                select good;
        }

        public IEnumerable<string>getMyGood(string name, IEnumerable<shop> Shops1)
        {
            var Shops = Shops1.ToList();

            return
                Shops.Where(s => s.Goods.FirstOrDefault(g => g.Name == name) != null).Select(sh => sh.Name);
        }

        public IEnumerable<shop> getBigShops(IEnumerable<shop> shops)
        {
            return shops.Where(p => p.Goods.Count > 5);
        }

        public ICollection<good> GetSale(shop Shop)
        {
            var goods = Shop.Goods;
            foreach (var i in goods)
            {
                i.Price *= Convert.ToSingle(Shop.Sale() / 100.0);
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