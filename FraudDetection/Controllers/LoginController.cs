using AdminAuth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdminAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;

        //Inject the configuration into the Login controller
        public LoginController(IConfiguration config)
        {
               _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] AdminLogin adminLogin)
        {
            //authenticate login
            var admin = Authenticate(adminLogin);

            if (admin != null)
            {
                //generate a token if the instance of the admin login passes authentication
                var token = Generate(admin);
                return Ok ( new
                {
                    adminLogin.UserName,
                    adminLogin.Password,
                    token

                });
            }
            return Unauthorized();
        }

        //Generate method which performs the token generation function
        private string Generate(AdminModel admin) 
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, admin.UserName),
                new Claim(ClaimTypes.Email, admin.EmailAddress),
                new Claim(ClaimTypes.Role, admin.Role),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"],
                claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //Authentication method which performs the authenticate funtion
        private AdminModel Authenticate(AdminLogin adminLogin)
        {
            var currentadmin = AdminInfo.Admins.FirstOrDefault(a => a.UserName.ToLower() == adminLogin.UserName.ToLower()
            && a.Password.ToLower() == adminLogin.Password.ToLower());

            if (currentadmin != null)
            {
                return currentadmin;
            }

            return null;
        }
    }
}
