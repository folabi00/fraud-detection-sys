using AdminAuth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
/*using System.Web.Http;*/

namespace AdminAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet("IsAdmin")]
        [Authorize(Roles ="Administrator")]
        public IActionResult AdminAuthenticated()
        {
            var AdminAuthentication = GetAdminAuthentication();
            return Ok($" {AdminAuthentication.UserName} is authorized");
        }
       
        private Admin GetAdminAuthentication() 
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var adminClaims = identity.Claims;
                return new Admin
                {
                    UserName = adminClaims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value,
                    EmailAddress = adminClaims.FirstOrDefault(a => a.Type == ClaimTypes.Email)?.Value,
                    Role = adminClaims.FirstOrDefault(a => a.Type == ClaimTypes.Role)?.Value
                };
            }

            return null; 
        }    
    }
}
