using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiMobile.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Text;
using ApiMobile.Services;
using ApiMobile.Helpers;
using Microsoft.Extensions.Options;

namespace ApiMobile.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribesController : ControllerBase
    {
        private readonly Context _context;
        private IUserService _userService;
        private readonly AppSettings _appSettings;

        public SubscribesController(Context context, IUserService userService, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _userService = userService;
            _appSettings = appSettings.Value;
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