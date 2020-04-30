﻿using System;
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
        public async Task<ActionResult<IEnumerable<good>>> GetGoods()
        {
            return await _context.Goods.ToListAsync();
        }

        // GET: api/goods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<good>> Getgood(long id)
        {
            var good = await _context.Goods.FindAsync(id);

            if (good == null)
            {
                return NotFound();
            }

            return good;
        }

        [HttpGet("Cheap")]
        [Authorize]
        public IEnumerable<good> GetCheapGoods()
        {
            return _context.getCheapGoods(_context.Goods);
            
        }

        [HttpGet("Find/{good}")]
        public IEnumerable<string> GetMyGood(string good)
        {
            return _context.getMyGood(good, _context.Shops);
            
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
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<good>> Postgood(good good)
        {
            _context.Goods.Add(good);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("Getgood", new { id = good.Id }, good);
            return CreatedAtAction(nameof(Getgood), new { id = good.Id }, good);
        }

        // DELETE: api/goods/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<good>> Deletegood(long id)
        {
            var good = await _context.Goods.FindAsync(id);
            if (good == null)
            {
                return NotFound();
            }

            _context.Goods.Remove(good);
            await _context.SaveChangesAsync();

            return good;
        }

        private bool goodExists(long id)
        {
            return _context.Goods.Any(e => e.Id == id);
        }
    }
}
