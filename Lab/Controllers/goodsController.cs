using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab.Models;
using Microsoft.AspNetCore.Authorization;

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
        [HttpPut("{id}")]
        public async Task<IActionResult> Putgood(long id, good good)
        {
            if (id != good.Id)
            {
                return BadRequest();
            }

            _context.Entry(good).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!goodExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

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

            _context.Shops.Where(s => s.Goods.FirstOrDefault(g => g.Id == id) != null).FirstOrDefault().Goods.Remove(good);
            await _context.SaveChangesAsync();

            return good;
        }

        private bool goodExists(long id)
        {
            return _context.getAllGoods().Any(e => e.Id == id);
        }
    }
}
