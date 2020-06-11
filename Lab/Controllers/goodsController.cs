using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Internal;

namespace Lab.Controllers
{
    [Route("api/goods")]
    [ApiController]
    public class goodsController : ControllerBase
    {
        private readonly mainContext _context;

        public goodsController(mainContext context)
        {
            _context = context;
        }

        // GET: api/goods
        [HttpGet]
        public IEnumerable<good> GetGoods()
        {
            return _context.getAllGoods().ToList();
        }

        // GET: api/goods/5
        [HttpGet("{id}")]
        public good Getgood(long id)
        {
            return _context.getGood(id);
        }

        [HttpGet("Cheap")]
        [Authorize]
        public IEnumerable<good> GetCheapGoods()
        {
            return _context.getCheapGoods();
        }

        [HttpGet("Find/{goodName}")]
        public IEnumerable<string> GetMyGood(string goodName)
        {
            return _context.getGoodsShop(goodName);
            
        }

        // PUT: api/goods/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{idS}/{idG}")]
        public async Task<IActionResult> Putgood(long idS, long idG, good good)
        {
            var Shop = await _context.Shops.FindAsync(idS);

            if (Shop == null)
                return BadRequest();

            var currentGood = Shop.Goods.FirstOrDefault(g => g.Id == idG);
            int index = Shop.Goods.IndexOf(currentGood);
            Shop.Goods.Remove(currentGood);
            Shop.Goods.Insert(index, good);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/goods
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("toShop/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<good>> Postgood(good good, long id)
        {
            var Shop = await _context.Shops.FindAsync(id);

            if (Shop == null)
                return BadRequest();

            Shop.Goods.Add(good);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("Getgood", new { id = good.Id }, good);
            return CreatedAtAction(nameof(Getgood), new { id = good.Id }, good);
        }

        // DELETE: api/goods/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<good>> Deletegood(long id)
        {
            var good = _context.getGood(id);
            if (good == null)
            {
                return NotFound();
            }

            _context.Shops.FirstOrDefault(s => s.Goods.FirstOrDefault(g => g.Id == id) != null).Goods.Remove(good);
            await _context.SaveChangesAsync();

            return good;
        }

        private bool goodExists(long id)
        {
            return _context.getAllGoods().Any(e => e.Id == id);
        }
    }
}
