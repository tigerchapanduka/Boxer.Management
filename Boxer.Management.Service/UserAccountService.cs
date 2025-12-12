using Boxer.Data;
using Boxer.Management.Product.Data;
using Boxer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Boxer.Model.Dto;

namespace Boxer.Management.Service
{
    public class UserAccountService: IUserAccountService
    {

        private IConfiguration appConfig;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly UserManager<UserAccount> _userManager;

        public UserAccountService(IConfiguration config, UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager) {

            appConfig = config;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<UserAccountResponse> Save(UserAccountRequest userAccountRequest, DatabaseContext dbcontext)
        {

            try
            {
                UserAccount  userAccount = new UserAccount();
                var appSettings = appConfig.GetSection("AppSettings").Get<AppSettings>();
                userAccount.ModifiedById = new Guid(appSettings.UserId);
                userAccount.CreatedById = new Guid(appSettings.UserId);
                userAccount.UserName = userAccountRequest.userName;
                userAccount.Password = userAccountRequest.password;
                var result = await _userManager.CreateAsync(userAccount, userAccount.Password);
                if (result.Succeeded)
                {

                    return new UserAccountResponse(null,userAccountRequest.userName, new Guid(userAccount.Id));
                }
                StringBuilder errorBuilder = new StringBuilder();
                foreach (IdentityError err in result.Errors)
                {
                    errorBuilder.AppendLine(err.Description);
                }
                return new UserAccountResponse(errorBuilder.ToString(), null, null);
               // return StatusCode(StatusCodes.Status500InternalServerError, errorBuilder.ToString());
            }
            catch (Exception ex)
            {
                return new UserAccountResponse(ex.Message, null, null);
            }
        }

        public  async Task<loginDto> LoginUserAccount(UserAccount authRequest, DatabaseContext dbcontext)
        {

            try
            {


                var appSettings = appConfig.GetSection("AppSettings").Get<AppSettings>();
           
                var result = await _signInManager.PasswordSignInAsync(authRequest.UserName, authRequest.Password, false, lockoutOnFailure: true);
            
                if (result.Succeeded)
                {
                    loginDto loginDto = new loginDto(null,GenerateJWTToken(new UserAccount()));
                    return loginDto;

                }
                else
                {
                    loginDto loginDto = new loginDto("Login failed", null);
                    return loginDto;
                }
            }
            catch (Exception ex)
            {
                return new loginDto(ex.Message,null);
            }
        }
        public string GenerateJWTToken(UserAccount user)
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
