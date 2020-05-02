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

      //  public DbSet<good> Goods { get; set; }
        public DbSet<shop> Shops { get; set; }


        public IEnumerable<good> getAllGoods()
        {
            var res = new List<good>();
            foreach (var i in Shops.ToList())
            {
                res.AddRange(i.Goods);
            }
            return res;
        }

        public good getGood(long id)
        {
            foreach (var i in Shops.ToList())
            {
                var res = i.Goods.FirstOrDefault(g => g.Id == id);
                if (res != null)
                    return res;
            }
            return null;
        }

        public IEnumerable<good> getCheapGoods()
        {
            var goods = getAllGoods();
            return
                from good in goods
                where good.Price < 1000
                select good;
        }

        public IEnumerable<string>getGoodsShop(string name)
        {
            return Shops.Where(s => s.Goods.FirstOrDefault(g => g.Name == name) != null).Select(sh => sh.Name);
        }

        public IEnumerable<shop> getBigShops(int h)
        {
            return Shops.Where(p => p.Goods.Count > h);
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