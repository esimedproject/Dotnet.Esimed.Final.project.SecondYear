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
    public class PaymentsController : ControllerBase
    {
        private readonly Context _context;

        public PaymentsController(Context context)
        {
            _context = context;
        }

        // GET: api/Payments
        [HttpGet]
        public IEnumerable<Payments> GetPayment()
        {
            return _context.Payment;
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayments([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payments = await _context.Payment.FindAsync(id);

            if (payments == null)
            {
                return NotFound();
            }

            return Ok(payments);
        }

        // PUT: api/Payments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayments([FromRoute] int id, [FromBody] Payments payments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != payments.CId)
            {
                return BadRequest();
            }

            _context.Entry(payments).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentsExists(id))
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

        // POST: api/Payments
        [HttpPost]
        public async Task<IActionResult> PostPayments([FromBody] Payments payments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Payment.Add(payments);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayments", new { id = payments.CId }, payments);
        }

        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayments([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payments = await _context.Payment.FindAsync(id);
            if (payments == null)
            {
                return NotFound();
            }

            _context.Payment.Remove(payments);
            await _context.SaveChangesAsync();

            return Ok(payments);
        }

        private bool PaymentsExists(int id)
        {
            return _context.Payment.Any(e => e.CId == id);
        }
    }
}