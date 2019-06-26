using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiMobile.Models;
using Microsoft.AspNetCore.Authorization;

namespace ApiMobile.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribesController : ControllerBase
    {
        private readonly Context _context;

        public SubscribesController(Context context)
        {
            _context = context;
        }

        // GET: api/Subscribes
        [HttpGet]
        public IEnumerable<Subscribes> GetSubscribe()
        {
            return _context.Subscribe;
        }

        // GET: api/Subscribes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubscribes([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subscribes = await _context.Subscribe.FindAsync(id);

            if (subscribes == null)
            {
                return NotFound();
            }

            return Ok(subscribes);
        }

        // PUT: api/Subscribes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscribes([FromRoute] int id, [FromBody] Subscribes subscribes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subscribes.Id)
            {
                return BadRequest();
            }

            _context.Entry(subscribes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscribesExists(id))
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

        // POST: api/Subscribes
        [HttpPost]
        public async Task<IActionResult> PostSubscribes([FromBody] Subscribes subscribes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Subscribe.Add(subscribes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubscribes", new { id = subscribes.Id }, subscribes);
        }

        // DELETE: api/Subscribes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscribes([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subscribes = await _context.Subscribe.FindAsync(id);
            if (subscribes == null)
            {
                return NotFound();
            }

            _context.Subscribe.Remove(subscribes);
            await _context.SaveChangesAsync();

            return Ok(subscribes);
        }

        private bool SubscribesExists(int id)
        {
            return _context.Subscribe.Any(e => e.Id == id);
        }
    }
}