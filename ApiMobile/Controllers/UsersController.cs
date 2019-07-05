using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiMobile.Models;
using ApiMobile.Services;
using ApiMobile.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;

namespace ApiMobile.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly Context _context;
        private IUserService _userService;
        private readonly AppSettings _appSettings;

        public UsersController(Context context, IUserService userService, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            string adminstatus = User.FindFirst(ClaimTypes.Role)?.Value;
            if (adminstatus == "admin")
            {
                var users = _userService.GetAll();
                return Ok(users);
            }
            else return Unauthorized();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            string useremail = User.FindFirst(ClaimTypes.Name)?.Value;
            string adminstatus = User.FindFirst(ClaimTypes.Role)?.Value;
            if (int.Parse(useremail) == id || adminstatus == "Admin")
            {
                var user = _userService.GetById(id);
                return Ok(user);
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Unauthorized(); 
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] Users users)
        {
            string hash = (from i in _context.User where i.Email == users.Email select i.Password).FirstOrDefault();

            if (Salt.ComparePassword(hash, users.Password))
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

                return Ok(new Users{ Email = user.Email, AuthentificationKey = user.AuthentificationKey, Id = user.Id });
            }
            return BadRequest();
    }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody]Users user)
        {
            string adminstatus = User.FindFirst(ClaimTypes.Role)?.Value;
            if (adminstatus == "admin") { 
                try
            {
                string hash = user.Password;
                user.Password = Salt.GetPasswordHash(user.Password);
                _context.User.Add(user);

                _context.SaveChanges();

                user.Password = hash;

                Authenticate(user);

                user.Password = null; 

                return CreatedAtAction("GetUsers", new { id = user.Id }, user);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            }else return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]Users user)
        {
            string useremail = User.FindFirst(ClaimTypes.Name)?.Value;
            string adminstatus = User.FindFirst(ClaimTypes.Role)?.Value;
            if (int.Parse(useremail) == id || adminstatus == "Admin")
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != user.Id)
                {
                    return BadRequest();
                }

                if (user.Password == null)
                {
                    user.Password = (from c in _context.User where c.Id == id select c.Password).FirstOrDefault();
                }

                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(id))
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
            else return Unauthorized();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string useremail = User.FindFirst(ClaimTypes.Name)?.Value;
            string adminstatus = User.FindFirst(ClaimTypes.Role)?.Value;
            if (int.Parse(useremail) == id || adminstatus == "admin")
            {
                _userService.Delete(id);
                return Ok();
            }
            else return Unauthorized();            
        }
        private bool UsersExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

    }
}