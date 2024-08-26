using Chatbot_Web.Context;
using Chatbot_Web.DTOs.Authantication;
using Chatbot_Web.Entities;
using Chatbot_Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Chatbot_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthanticationController : ControllerBase
    {


        private readonly ChatbotContext _context;
        public AuthanticationController(ChatbotContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult SignUp([FromBody] SignupDTO input)
        {
            if (input == null)
                return BadRequest("Entity Is Empty");
            User client = new User()
            {
                Email = input.Email,
                FirstName = input.FirstName,
                LastName = input.LastName,
                Password = HashingHelper.GenerateSHA384String(input.Password),
                Username = HashingHelper.GenerateSHA384String(input.Email)
            };
            _context.Add(client);
            _context.SaveChanges();
            if (client.Id > 0)
            {
                Random random = new Random();
                VerificationCode verificationCode = new VerificationCode()
                {
                    Email = input.Email,
                    ExpireDate = DateTime.Now,
                    OTP = random.Next(111111, 999999).ToString()
                };
                _context.Add(verificationCode);
                _context.SaveChanges();
                EmailSender.SendOtpViaEmail(input.Email, verificationCode.OTP);
                return StatusCode(201);
            }
            else
                return StatusCode(503, "Something Went Wrong");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult SignIn([FromBody] SignInDTO input)
        {
            if (input == null)
                return BadRequest("SignIn Entity Is Empty");
            input.Password = HashingHelper.GenerateSHA384String(input.Password);
            input.UserName = HashingHelper.GenerateSHA384String(input.UserName);
            var client = _context.Users
                .Where(x => x.Username.Equals(input.UserName) && x.IsLoggedIn == false
                && x.Password.Equals(input.Password) && x.IsEmailVerified == true)
                .FirstOrDefault();
            if (client == null) return Unauthorized("You're not Allowed to login");

            else
            {
                var token = TokenHelper.GenerateJwtToken(client);
                client.IsLoggedIn = true;
                client.LastLoginDateTime = DateTime.Now;
                _context.Update(client);
                _context.SaveChanges();
                return StatusCode(200, token);
            }

        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult VerifyAccount([FromBody] AccountConfirmationDTO input)
        {
            if (input == null)
                return BadRequest("Verify Account Entity Is Empty");
            else
            {
                var client = _context.Users
               .Where(x => x.Email == input.Email && x.IsEmailVerified == false)
               .FirstOrDefault();
                if (client == null)
                    return BadRequest("Colud not verify Accounnt");
                else
                {
                    var code = _context.VerificationCodes.Where(
                        x => x.OTP == input.Code && x.Email == input.Email
                        && x.ExpireDate > DateTime.Now).FirstOrDefault();
                    if (code != null)
                    {
                        client.IsEmailVerified = true;
                        _context.Update(client);
                        _context.SaveChanges();
                        _context.Remove(code);
                        _context.SaveChanges();
                        return StatusCode(200);
                    }
                    return BadRequest("Colud not verify Accounnt");
                }
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDTO input)
        {
            if (input == null)
                return BadRequest("Reset Password DTO  Entity Is Empty");
            else
            {
                var client = _context.Users
               .Where(x => x.Email == input.Email).FirstOrDefault();
                if (client == null)
                    return BadRequest("Colud not verify Accounnt");
                else
                {
                    if (input.NewPassword.Equals(input.ConfirmPassword))
                    {
                        client.Password = HashingHelper.GenerateSHA384String(input.NewPassword);
                        _context.Update(client);
                        _context.SaveChanges();
                    }
                    return BadRequest("Colud not verify Accounnt");

                }
            }
        }
    }
}
