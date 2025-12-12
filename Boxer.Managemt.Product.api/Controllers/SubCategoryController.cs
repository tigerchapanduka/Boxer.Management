using Boxer.Data;
using Boxer.Management.Service;
using Boxer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Boxer.Managemt.Product.api.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    
    public class SubCategoryController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private IConfiguration appConfig;
        //private IPasswordHasher<UserAccount> passwordHasher;
   
        public SubCategoryController(DatabaseContext context, IConfiguration config)
        {
            _context = context;
            appConfig = config;
   
        }

        // GET: api/UserAccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAccount>>> GetUserAccount()
        {
            String requestedpeth = Request.Path.Value;
            return await _context.UserAccount.ToListAsync();
        }

        // GET: api/UserAccounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserAccount>> GetUserAccount(String id)
        {
            var userAccount = await _context.UserAccount.FindAsync(id);

            if (userAccount == null)
            {
                return NotFound();
            }

            return userAccount;
        }

        // PUT: api/UserAccounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserAccount(String id, UserAccount userAccount)
        {
            if (id != userAccount.Id)
            {
                return BadRequest();
            }

            _context.Entry(userAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAccountExists(id))
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

        // POST: api/UserAccounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<String>> PostUserAccount(UserAccount userAccount)
        {
            try
            {
                var appSettings = appConfig.GetSection("AppSettings").Get<AppSettings>();
                userAccount.PasswordHash = passwordService.HashPassword(userAccount, "password");
                Boolean isVerified =   passwordService.VerifyPassword(userAccount, userAccount.PasswordHash, "password");
                userAccount.ModifiedById = new Guid(appSettings.UserId);
                userAccount.CreatedById = new Guid(appSettings.UserId);
                _context.UserAccount.Add(userAccount);
                await _context.SaveChangesAsync();
                return GenerateJWTToken(userAccount);
                //return CreatedAtAction("GetUserAccount", new { id = userAccount.Id }, userAccount);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE: api/UserAccounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAccount(String id)
        {
            var userAccount = await _context.UserAccount.FindAsync(id);
            if (userAccount == null)
            {
                return NotFound();
            }

            _context.UserAccount.Remove(userAccount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserAccountExists(String id)
        {
            return _context.UserAccount.Any(e => e.Id == id);
        }

        public string GenerateJWTToken(UserAccount user)
        {
            var claims = new List<Claim> {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
    };
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(appConfig["AppSettings:JWT_Secret"])
                        ),
                    SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
