using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TaskManagement.Api.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost]
        public ActionResult<string> Authenticate([FromBody] AuthenticationRequest request)
        {

            // step 1 : validate userName and password
            var user = ValidateUser(request.UserName, request.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            //step 2 : Generate Security Key from base64 string text in appsettings

            var key = _configuration.GetValue<string>("Authentication:SecretKey")
               ?? throw new Exception(" secret key not found"); //Configuration exception

            byte[] bytes = Convert.FromBase64String(key);

            var securitykey = new SymmetricSecurityKey(bytes);

            // step 3 : generate signing credentials 

            var signingCredentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new List<Claim>
            {
                new ("sub",user.UserId.ToString()),
                new ("given_name",user.FirstName.ToString()),
                new ("family_name",user.LastName.ToString()),
                new ("preferred_username",user.UserName.ToString())

            };

            // step 4 : generate JWT token

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Authentication:Issuer"),
                audience: _configuration.GetValue<string>("Authentication:Audience"),
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signingCredentials
                );
            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(token);



        }
        private ApplicationUser ValidateUser(string userName, string password)
        {
            if (userName == "test@gmail.com" && password == "123")
            {
                return new ApplicationUser
                {
                    UserId = 1,
                    FirstName = "First",
                    LastName = "Last",
                    UserName = "test@gmail.com"

                };
            }
            return null;
        }
    }
    public class ApplicationUser
    {
        public required int UserId { get; set; }
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }


    }

    public sealed class AuthenticationRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
