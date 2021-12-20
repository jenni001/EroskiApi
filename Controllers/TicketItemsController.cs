using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiEroski.Models;

namespace ApiEroski.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketItemsController : ControllerBase
    {
        private readonly TicketContext _context;

        public TicketItemsController(TicketContext context)
        {
            _context = context;
        }

        // GET: api/TicketItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketItem>>> GetTicketItem()
        {
            return await _context.TicketsItem.ToListAsync();
        }

        // GET: api/TicketItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketItem>> GetTicketItem(string id)
        {
            var ticketItem = await _context.TicketsItem.FindAsync(id);

            if (ticketItem == null)
            {
                return NotFound();
            }

            return ticketItem;
        }

        [HttpGet("/reset/{id}")]
        public async Task<ActionResult> GetTicketItemReset(string id)
        {
            var item = await _context.TicketsItem
              .FirstOrDefaultAsync(t => t.Nombre == id);
            item.NumTicket = 0;
            await _context.SaveChangesAsync();
            return Ok(new
                {
                    ResetOn = DateTime.Now,
                    item = item
                });

        }

        [HttpGet("/reset")]
        public async Task<ActionResult> GetTicketsItemReset()
        {
            foreach (var item in  _context.TicketsItem)
            {
                item.NumTicket=0;
            }
            
            await _context.SaveChangesAsync();
            return Ok(new
                {
                    ResetOn = DateTime.Now
                });

        }



        // PUT: api/TicketItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicketItem(string id)
        {
            var ticketItem = await _context.TicketsItem.FindAsync(id);

            ticketItem.NumTicket++;

            _context.Entry(ticketItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketItemExists(id))
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

        // POST: api/TicketItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TicketItem>> PostTicketItem(TicketItem ticketItem)
        {
            _context.TicketsItem.Add(ticketItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TicketItemExists(ticketItem.Nombre))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTicketItem", new { id = ticketItem.Nombre }, ticketItem);
        }

        // DELETE: api/TicketItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicketItem(string id)
        {
            var ticketItem = await _context.TicketsItem.FindAsync(id);
            if (ticketItem == null)
            {
                return NotFound();
            }

            _context.TicketsItem.Remove(ticketItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketItemExists(string id)
        {
            return _context.TicketsItem.Any(e => e.Nombre == id);
        }
    }
}
