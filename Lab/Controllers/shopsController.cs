using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab.Models;

namespace Lab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class shopsController : ControllerBase
    {
        private readonly shopContext _context;

        public shopsController(shopContext context)
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
        public async Task<ActionResult<int>> Getshop(long id)
        {
            var shop = await _context.Shops.FindAsync(id);

            if (shop == null)
            {
                return NotFound();
            }

            return shop.Sale();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<shop>> GetSale(long id)
        {
            var shop = await _context.Shops.FindAsync(id);

            if (shop == null)
            {
                return NotFound();
            }

            return shop;
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
        public async Task<ActionResult<shop>> Postshop(shop shop)
        {
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("Getshop", new { id = shop.Id }, shop);
            return CreatedAtAction(nameof(Getshop), new { id = shop.Id }, shop);
        }

        // DELETE: api/shops/5
        [HttpDelete("{id}")]
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
