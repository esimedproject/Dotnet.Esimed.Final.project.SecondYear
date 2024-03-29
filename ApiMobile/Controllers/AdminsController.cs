﻿using System;
using Microsoft.AspNetCore.Mvc;
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
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ApiMobile.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly Context _context;
        private IAdminService _adminService;
        private readonly AppSettings _appSettings;

        public AdminsController(Context context, IAdminService adminService, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _adminService = adminService;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            string adminstatus = User.FindFirst(ClaimTypes.Role)?.Value;
            if (adminstatus == "admin")
            {
                var admins = _adminService.GetAllAdmins();
                return Ok(admins);
            }
            else return Unauthorized();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            string adminstatus = User.FindFirst(ClaimTypes.Role)?.Value;
            if (adminstatus == "admin")
            {
                var admin = _adminService.GetByAdminId(id);
                return Ok(admin);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("AuthenticateAdmin")]
        public IActionResult AuthenticateAdmin([FromBody] Admins admins)
        {
            string hash = (from i in _context.Admin where i.Email == admins.Email select i.Password).FirstOrDefault();


            if (Salt.ComparePassword(hash, admins.Password))
            {
                var admin = _adminService.AuthenticateAdmin(admins.Email, admins.Password);

                if (admin == null)
                    return BadRequest(new { message = "Email or password is incorrect" });

                var tokenHandlerad = new JwtSecurityTokenHandler();
                var keyad = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptoradmin = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                                        new Claim(ClaimTypes.Name, admin.Id.ToString()),
                                        new Claim(ClaimTypes.Role, "admin")
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyad), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenad = tokenHandlerad.CreateToken(tokenDescriptoradmin);
                string tokenStringad = tokenHandlerad.WriteToken(tokenad);

                admin.AuthentificationKey = tokenStringad;
                _adminService.GetContextAdmins().SaveChangesAsync();


                return Ok(new Admins { Email = admin.Email, AuthentificationKey = admin.AuthentificationKey, Id = admin.Id });
            }
                return BadRequest(); 
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody]Admins admin)
        {
            string adminstatus = User.FindFirst(ClaimTypes.Role)?.Value;
            if (adminstatus == "admin") { 
                try
                {
                    string hash = admin.Password;
                    admin.Password = Salt.GetPasswordHash(admin.Password);
                    _context.Admin.Add(admin);

                    _context.SaveChanges();

                    admin.Password = hash;

                    AuthenticateAdmin(admin);

                    admin.Password = null;

                    return CreatedAtAction("GetUsers", new { id = admin.Id }, admin);
                }
                catch (AppException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }else return Unauthorized();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, [FromBody]Admins admin)
        {
            string useremail = User.FindFirst(ClaimTypes.Name)?.Value;
            string adminstatus = User.FindFirst(ClaimTypes.Role)?.Value;
            if (int.Parse(useremail) == id || adminstatus == "Admin")
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != admin.Id)
                {
                    return BadRequest();
                }

                if (admin.Password == null)
                {
                    admin.Password = (from c in _context.Admin where c.Id == id select c.Password).FirstOrDefault();
                }

                _context.Entry(admin).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminsExists(id))
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
        public IActionResult DeleteAdmin(int id)
        {
            string adminstatus = User.FindFirst(ClaimTypes.Role)?.Value;
            if (adminstatus == "admin")
            {
                _adminService.DeleteAdmin(id);
                return Ok();
            }
            return Unauthorized();
        }
        private bool AdminsExists(int id)
        {
            return _context.Admin.Any(e => e.Id == id);
        }

    }
}