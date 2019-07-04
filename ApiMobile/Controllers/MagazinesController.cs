using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiMobile.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System;
using ApiMobile.Services;
using ApiMobile.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ApiMobile.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class MagazinesController : ControllerBase
    {
        private readonly Context _context;
        private IUserService _userService;
        private readonly AppSettings _appSettings;

        public MagazinesController(Context context, IUserService userService, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        // GET: api/Magazines
        [HttpGet]
        public IActionResult GetMagazine()
        {
            var mag = from i in _context.Magazine orderby i.Nom descending select i;
            return Ok(mag);
            //string adminstatus = User.FindFirst(ClaimTypes.Role)?.Value;
            //if (adminstatus == "admin")
            //{

            //}
            //else return Unauthorized();
        }

        // GET: api/Magazines/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMagazines([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var magazines = await _context.Magazine.FindAsync(id);

            if (magazines == null)
            {
                return NotFound();
            }

            return Ok(magazines);
        }


        // GET: api/Magazines/ByUser
        [HttpGet("ByUser/{id}")]
        public IActionResult GetByUserMagazines(int id)
        {
            string useremail = User.FindFirst(ClaimTypes.Name)?.Value;
            if (int.Parse(useremail) == id)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var UserMagazine = from a in _context.Magazine
                                   join b in _context.Subscribe on a.SubscribesMagazineID
                                   equals b.Id
                                   where b.UserSubscribeID == id
                                   where b.End_date_subscribe > DateTime.Now
                                   select a;

                if (UserMagazine == null)
                {
                    return NotFound();
                }

                return Ok(UserMagazine);
            }
            else return Unauthorized();
        }

        // GET: api/Magazines/ByUser
        [HttpGet("ByNotUser/{id}")]
        public IActionResult GetByUserNotSubscribes(int id)
        {
            string useremail = User.FindFirst(ClaimTypes.Name)?.Value;
            if (int.Parse(useremail) == id)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var UserMagazine = from x in _context.Magazine
                                   join v in _context.Subscribe on x.SubscribesMagazineID
                                   equals v.Id
                                   where !(from a in _context.Magazine
                                           join b in _context.Subscribe on a.SubscribesMagazineID
                                           equals b.Id
                                           where b.UserSubscribeID == id
                                           where b.End_date_subscribe > DateTime.Now
                                           select a.Id).Contains(x.Id)
                                   select x;

                if (UserMagazine == null)
                {
                    return NotFound();
                }

                return Ok(UserMagazine);
            }
            else return Unauthorized();
        }

        // PUT: api/Magazines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMagazines([FromRoute] int id, [FromBody] Magazines magazines)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != magazines.Id)
            {
                return BadRequest();
            }

            _context.Entry(magazines).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MagazinesExists(id))
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

        // POST: api/Magazines
        [HttpPost]
        public async Task<IActionResult> PostMagazines([FromBody] Magazines magazines)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Magazine.Add(magazines);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMagazines", new { id = magazines.Id }, magazines);
        }

        // DELETE: api/Magazines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMagazines([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var magazines = await _context.Magazine.FindAsync(id);
            if (magazines == null)
            {
                return NotFound();
            }

            _context.Magazine.Remove(magazines);
            await _context.SaveChangesAsync();

            return Ok(magazines);
        }

        private bool MagazinesExists(int id)
        {
            return _context.Magazine.Any(e => e.Id == id);
        }
    }
}