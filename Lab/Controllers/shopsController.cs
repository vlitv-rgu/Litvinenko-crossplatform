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
    [Route("api/shops")]
    [ApiController]
    public class shopsController : ControllerBase
    {
        private readonly mainContext _context;

        public shopsController(mainContext context)
        {
            _context = context;
        }

        // GET: api/shops
        [HttpGet]
        public async Task<ActionResult<IEnumerable<shop>>> GetShops()
        {
            return await _context.Shops.ToListAsync();
        }

        // GET: api/shops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<shop>> Getshop(long id)
        {
            var shop = await _context.Shops.FindAsync(id);

            if (shop == null)
            {
                return NotFound();
            }

            return shop;
        }

        [HttpGet("{id}/Sale")]
        [Authorize]
        public ICollection<good> GetSale(long id)
        {
            var shop = _context.Shops.Find(id);

            if (shop == null)
            {
                return null;
            }
            return _context.GetSale(shop);
        }

        [HttpGet("Bigs/{h}")]
        [Authorize]
        public IEnumerable<shop> GetBigShops(int h)
        {
            return _context.getBigShops(h);

        }

        // PUT: api/shops/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putshop(long id, shop shop)
        {
            if (id != shop.Id)
            {
                return BadRequest();
            }

            _context.Entry(shop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!shopExists(id))
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

        // POST: api/shops
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<shop>> Postshop(shop shop)
        {
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getshop", new { id = shop.Id }, shop);
        }

        // DELETE: api/shops/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<shop>> Deleteshop(long id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop == null)
            {
                return NotFound();
            }

            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();

            return shop;
        }

        private bool shopExists(long id)
        {
            return _context.Shops.Any(e => e.Id == id);
        }
    }
}
