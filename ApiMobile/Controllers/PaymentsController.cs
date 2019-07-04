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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        public IActionResult GetPayment()
        {
            string adminstatus = User.FindFirst(ClaimTypes.Role)?.Value;
            if (adminstatus == "admin")
            {
                var pay = from i in _context.Payment orderby i.CId descending select i;
                return Ok(pay);
            }
            else return Unauthorized();
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayments([FromRoute] int id)
        {
            string useremail = User.FindFirst(ClaimTypes.Name)?.Value;
            string adminstatus = User.FindFirst(ClaimTypes.Role)?.Value;
            if (int.Parse(useremail) == id || adminstatus == "Admin")
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
            else return Unauthorized();
        }

        // GET: api/Payments/ByUser/5
        [HttpGet("ByUser/{id}")]
        public IActionResult GetById(int id)
        {
            string useremail = User.FindFirst(ClaimTypes.Name)?.Value;
            string adminstatus = User.FindFirst(ClaimTypes.Role)?.Value;
            if (int.Parse(useremail) == id || adminstatus == "Admin")
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var UserPayment = from a in _context.Payment
                                  join b in _context.Subscribe on a.SubscribesPaymentID
                                  equals b.Id
                                  where b.UserSubscribeID == id
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
            string useremail = User.FindFirst(ClaimTypes.Name)?.Value;
            string adminstatus = User.FindFirst(ClaimTypes.Role)?.Value;
            if (int.Parse(useremail) == id || adminstatus == "Admin")
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
            return Unauthorized();
            
        }

        //POST: api/Payments/access
        [HttpPost("Access/{idUser}/{idMagazine}")]
        public IActionResult Access([FromBody] Payments payments, int idUser,int idMagazine)
        {
            string useremail = User.FindFirst(ClaimTypes.Name)?.Value;
            string adminstatus = User.FindFirst(ClaimTypes.Role)?.Value;
            if (int.Parse(useremail) == idUser || adminstatus == "Admin")
            {
                RestClient ClientRest = new RestClient(new Uri(@"ec2-52-47-88-142.eu-west-3.compute.amazonaws.com"));
                RestRequest RequestRest = new RestRequest($"{payments.MeansOfPayment}/e7597a36-673b-caeb-2675-a4f65902dd13/{payments.CId}/{payments.cardid}/{payments.cardmonth}/{payments.cardyear}/{payments.PaymentAmount.ToString().Replace(",", ".")}", Method.GET);
                var response = ClientRest.Execute(RequestRest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    var objet = new Payments { MeansOfPayment = payments.MeansOfPayment, PaymentAmount = payments.PaymentAmount, CId = payments.CId, SubscribesPaymentID = int.Parse(useremail) };
                    _context.Payment.Add(objet);

                    _context.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest(response.ErrorException);
            }return Unauthorized();
        }

        //POST: api/Payments/Refund
        [HttpPost("Refund")]
        public IActionResult Refund([FromBody] Payments payments)
        {
            RestClient ClientRest = new RestClient(new Uri(@"ec2-52-47-88-142.eu-west-3.compute.amazonaws.com"));
            RestRequest RequestRest = new RestRequest($"{payments.MeansOfPayment}/e7597a36-673b-caeb-2675-a4f65902dd13/{payments.transaction.ToString().Replace(",", ".")}", Method.GET);
            var response = ClientRest.Execute(RequestRest);
            return response.StatusCode == System.Net.HttpStatusCode.OK ? Ok() : (IActionResult)BadRequest(response.ErrorException);
        }

        //POST: api/Payments/PartialRefund
        [HttpPost("PartialRefund")]
        public IActionResult PartialRefund([FromBody] Payments payments)
        {
            RestClient ClientRest = new RestClient(new Uri(@"ec2-52-47-88-142.eu-west-3.compute.amazonaws.com"));
            RestRequest RequestRest = new RestRequest($"{payments.MeansOfPayment}/e7597a36-673b-caeb-2675-a4f65902dd13/{payments.transaction}/{payments.PaymentAmount.ToString().Replace(",", ".")}", Method.GET);
            var response = ClientRest.Execute(RequestRest);
            return response.StatusCode == System.Net.HttpStatusCode.OK ? Ok() : (IActionResult)BadRequest(response.ErrorException);
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<IActionResult> PostPayments(string typeofpaiment, long amount, int transac, int id)
        {
            string useremail = User.FindFirst(ClaimTypes.Name)?.Value;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }else
            {

                //_context.Payment.add();

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