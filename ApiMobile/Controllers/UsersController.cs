using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using ApiMobile.Models;
using ApiMobile.Services;
using ApiMobile.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Data.SqlClient;

namespace ApiMobile.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private readonly AppSettings _appSettings;
        

        public UsersController(IUserService userService, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            string useremail = User.FindFirst(ClaimTypes.Name)?.Value;
            if(int.Parse(useremail) == id)
            {
                var user = _userService.GetById(id);
                return Ok(user);
            }
            return Ok(); 
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
            
            return Ok(new {Token = tokenString, ID = user.Id});
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]Users user)
        {
            try
            {
                _userService.Create(user, user.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]Users user)
        {
            user.Id = id;
            string useremail = User.FindFirst(ClaimTypes.Name)?.Value;
            if (int.Parse(useremail) == id)
            {
                try
                {
                    _userService.Update(user, user.Password);
                    return Ok();
                }
                catch (AppException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
            return Ok();                   
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string useremail = User.FindFirst(ClaimTypes.Name)?.Value;
            if (int.Parse(useremail) == id)
            {
                _userService.Delete(id);
                return Ok();
            }
            return Ok();            
        }

    }
}