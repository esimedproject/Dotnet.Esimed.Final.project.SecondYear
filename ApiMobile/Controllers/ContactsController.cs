using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiMobile.Models;

namespace ApiMobile.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly Context _context;

        public ContactsController(Context context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        public IEnumerable<Contacts> GetContact()
        {
            return _context.Contact;
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContacts([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contacts = await _context.Contact.FindAsync(id);

            if (contacts == null)
            {
                return NotFound();
            }

            return Ok(contacts);
        }

        // PUT: api/Contacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContacts([FromRoute] int id, [FromBody] Contacts contacts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contacts.Id)
            {
                return BadRequest();
            }

            _context.Entry(contacts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactsExists(id))
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

        // POST: api/Contacts
        [HttpPost]
        public async Task<IActionResult> PostContacts([FromBody] Contacts contacts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Contact.Add(contacts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContacts", new { id = contacts.Id }, contacts);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContacts([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contacts = await _context.Contact.FindAsync(id);
            if (contacts == null)
            {
                return NotFound();
            }

            _context.Contact.Remove(contacts);
            await _context.SaveChangesAsync();

            return Ok(contacts);
        }

        private bool ContactsExists(int id)
        {
            return _context.Contact.Any(e => e.Id == id);
        }
    }
}