using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using InventoryManagement.Services;
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
        private readonly InventoryContext _context;
        public AuthController(IConfiguration configuration, InventoryContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public static EntityFrameworkCore.User userTemp = new EntityFrameworkCore.User();
       
        [HttpPost("CreateUser")]
        public async Task<ActionResult<User>> CreateUser(User request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.userName == request.userName);

            if (user != null)
            {
                return BadRequest("User already exists");
            }

            CreatePasswordHash(request.password, out byte[] passwordHash,out byte[] passwordSalt);
          
            var dbUser = new EntityFrameworkCore.User();

            userTemp.userName = request.userName;//to be removed later added because hashed password restore is causing issues
            userTemp.passwordHash = passwordHash;//to be removedlater added because hashed password restore causing issues
            userTemp.passwordSalt = passwordSalt; //to be removedlater added because hashed password restore causing issues

            dbUser.userName=request.userName;
            dbUser.passwordHash = passwordHash;
            dbUser.passwordSalt = passwordSalt;

             _context.Users.Add(dbUser);

            await _context.SaveChangesAsync();

            return Ok(request);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(User request)
        {
            bool isAdmin=false;

            var user = await _context.Users.FirstOrDefaultAsync(x => x.userName == request.userName);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            //if (!VerifyPasswordHash(request.password, userTemp.passwordHash, userTemp.passwordSalt)) //stored in temporary entity model 
            if (!VerifyPasswordHash(request.password, user.passwordHash, user.passwordSalt))     //get rid of db storing and use this one
            {
                return (BadRequest("Wrong password"));
            }

            if (request.userName == "Admin")
            {
                isAdmin = true;
            }

            string token = CreateToken(user, isAdmin);
            return Ok(token);
        }

        private string CreateToken(EntityFrameworkCore.User user, bool isAdmin )
        {
            List<Claim> claims;
            if (isAdmin==true)
            {
                claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.userName),
                    new Claim(ClaimTypes.Role, "Admin")
                };
            }
            else
            {
                claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.userName),
                    new Claim(ClaimTypes.Role, "Customer")
                };
            }
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
            using var hmac = new HMACSHA512(passwordSalt);
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computeHash.SequenceEqual(passwordHash);
        }

    }
}
