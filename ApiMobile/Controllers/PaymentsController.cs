using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiMobile.Models;
using ApiMobile.Services;
using Microsoft.Extensions.Options;
using ApiMobile.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using RestSharp; 

namespace ApiMobile.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly Context _context;
        private IUserService _userService;
        private readonly AppSettings _appSettings;

        public PaymentsController(Context context, IUserService userService, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        // GET: api/Payments
        [HttpGet]
        public IEnumerable<Payments> GetPayment()
        {
            var pay = from i in _context.Payment orderby i.CId descending select i;
            return pay;
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

        // GET: api/Magazines/ByUser
        [HttpGet("ByUser/{id}")]
        public IActionResult GetById(int id)
        {
            string useremail = User.FindFirst(ClaimTypes.Name)?.Value;
            if (int.Parse(useremail) == id)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var UserPayment = from a in _context.Payment
                                   join b in _context.Subscribe on a.SubscribesPaymentID
                                   equals b.Id
                                   where b.UserSubscribeID == id
                                   where b.End_date_subscribe > DateTime.Now
                                   select a;


                if (UserPayment == null)
                {
                    return NotFound();
                }

                return Ok(UserPayment);
            }
            else return Unauthorized();
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

        // POST: api/Payments/Authenticate
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

        //POST: api/Payments/access
        [HttpPost("Access")]
        public IActionResult Access([FromBody] Payments payments)
        {
            RestClient ClientRest = new RestClient(new Uri(@"http://192.168.2.1:6543"));
            RestRequest RequestRest = new RestRequest($"{payments.MeansOfPayment}/e7597a36-673b-caeb-2675-a4f65902dd13/{payments.CId}/{payments.cardid}/{payments.cardmonth}/{payments.cardyear}/{payments.PaymentAmount}", Method.GET);
            var response = ClientRest.Execute(RequestRest);
            return response.StatusCode == System.Net.HttpStatusCode.OK ? Ok() : (IActionResult)BadRequest(response.ErrorException);
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<IActionResult> PostPayments(string typeofpaiment,long amount,int transac,int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }else
            {
                typeofpaiment = "cardpay"; 
                var objet = new Payments { MeansOfPayment = typeofpaiment, PaymentAmount = amount, transaction = transac, CId = id  };
                _context.Payment.Add(objet);

                await _context.SaveChangesAsync();
                return Ok();
            }
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