using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using User = InventoryManagement.Contracts.User;

namespace InventoryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static EntityFrameworkCore.User user = new EntityFrameworkCore.User();
       
        [HttpPost("CreateUser")]
        public async Task<ActionResult<User>> CreateUser(User request)
        {
            CreatePasswordHash(request.password, out byte[] passwordHash,out byte[] passwordSalt);

            user.userName=request.userName;
            user.passwordHash = passwordHash;
            user.passwordSalt = passwordSalt;
            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(User request)
        {
            if (user.userName != request.userName)
            {
                return BadRequest("User not found");
            }

            if (!VerifyPasswordHash(request.password, user.passwordHash, user.passwordSalt))
            {
                return (BadRequest("Wrong password"));
            }

            string token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(EntityFrameworkCore.User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.userName),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value)); //can be injected through configuration

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
          
            var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.Now.AddDays(1),
               signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(user.passwordSalt);
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computeHash.SequenceEqual(passwordHash);
        }

    }
}
