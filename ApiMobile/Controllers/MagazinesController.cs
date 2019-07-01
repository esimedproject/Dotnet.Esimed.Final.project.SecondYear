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

        // GET: api/Magazines
        [HttpGet]
        public IEnumerable<Magazines> GetMagazine()
        {
            var mag = from i in _context.Magazine orderby i.Nom descending select i; 
            return mag; 
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

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] Users users)
        {
            var user = _userService.Authenticate(users.Email, users.Password);

            if (user == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            user.AuthentificationKey = tokenString;
            _userService.GetContext().SaveChangesAsync();

            return Ok(new { Token = tokenString, ID = user.Id });
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