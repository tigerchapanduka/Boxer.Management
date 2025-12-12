using Boxer.Data;
using Boxer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Boxer.Management.Security.Authentication.Service;
using static Boxer.Model.Dto;
using db=Boxer.Management.Security.Authentication.Data;


namespace Boxer.Management.Security.Authentication.api.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountsController : ControllerBase
    {
        private readonly db.DatabaseContext _context;
        private IConfiguration appConfig;
        private IPasswordService passwordService;
        private IUserAccountService _userAccountService;
        private SignInManager<UserAccount> _signInManager;
        private UserManager<UserAccount> _userManager;

        public UserAccountsController(
            db.DatabaseContext context, 
            IConfiguration config,
            IPasswordService passwordservice,
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager,
            IUserAccountService userAccountService
            )
        {
            _context = context;
            appConfig = config;
            passwordService = passwordservice;
            _userManager = userManager;
            _signInManager = signInManager;
            _userAccountService = userAccountService;


        }

        // GET: api/UserAccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAccount>>> GetUserAccount()
        {
            return await _context.UserAccount.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserAccount>> GetUserAccount(Guid id)
        {
            var userAccount = await _context.UserAccount.FindAsync(id);

            if (userAccount == null)
            {
                return NotFound();
            }

            return userAccount;
        }

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

        [HttpPost]
        public async Task<ActionResult<String>> PostUserAccount(UserAccount userAccount)
        {
            try
            {
                var appSettings = appConfig.GetSection("AppSettings").Get<AppSettings>();
                userAccount.ModifiedById = new Guid(appSettings.UserId);
                userAccount.CreatedById = new Guid(appSettings.UserId);

                UserAccountResponse loginDto = await _userAccountService.Save(new UserAccountRequest(userAccount.UserName,userAccount.Password), _context,_userManager);
                if (loginDto != null)
                {
                    return StatusCode(StatusCodes.Status200OK, loginDto);

                }
                else {
                    return StatusCode(StatusCodes.Status500InternalServerError, "User account creation Error occured");

                }


            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAccount(Guid id)
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

        [HttpPost("LoginUserAccount")]
        public async Task<ActionResult<String>> LoginUserAccount(AuthenticationRequest authRequest)
        {
            try
            {

                var appSettings = appConfig.GetSection("AppSettings").Get<AppSettings>();

                loginResponse loginDto = await _userAccountService.LoginUserAccount(new AuthenticationRequest(authRequest.userName, authRequest.password), _context, _signInManager);

                if (loginDto != null)
                {
                    if (String.IsNullOrEmpty(loginDto.errorMessage))
                    {

                        return StatusCode(StatusCodes.Status200OK, new loginResponse(null,loginDto.userId, GenerateJWTToken(new UserAccount())));

                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status204NoContent);

                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Login Error occured");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        private bool UserAccountExists(String id)
        {
            return _context.UserAccount.Any(e => e.Id == id);
        }

        private string GenerateJWTToken(UserAccount user)
        {
         //   var claims = new List<Claim> {
         //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
         //    new Claim(ClaimTypes.Name, user.UserName),
         //};

            var jwtToken = new JwtSecurityToken(
                //claims: claims,
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
