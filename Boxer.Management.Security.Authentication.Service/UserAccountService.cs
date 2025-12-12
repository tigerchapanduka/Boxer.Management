using Boxer.Data;

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
using Boxer.Management.Security.Authentication.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using System.Security.Claims;

namespace Boxer.Management.Security.Authentication.Service
{
    public class UserAccountService: IUserAccountService
    {

        private IConfiguration appConfig;
        private IPasswordService passwordService;

        public UserAccountService(IConfiguration config, IPasswordService passwordservice)
        {

            appConfig = config;
            this.passwordService = passwordservice;
        }


        public async Task<UserAccountResponse> Save(UserAccountRequest userAccountRequest,
            DatabaseContext dbcontext,UserManager<UserAccount> userManager)
        {

            try
            {
                UserAccount  userAccount = new UserAccount();
                var appSettings = appConfig.GetSection("AppSettings").Get<AppSettings>();
                userAccount.ModifiedById = new Guid(appSettings.UserId);
                userAccount.CreatedById = new Guid(appSettings.UserId);
                userAccount.UserName = userAccountRequest.userName;
                userAccount.Password = userAccountRequest.password;
                var result = await userManager.CreateAsync(userAccount, userAccount.Password);
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

        public  async Task<loginResponse> LoginUserAccount(AuthenticationRequest authRequest, DatabaseContext dbcontext, SignInManager<UserAccount> signInManager)
        {

            try
            {


                var appSettings = appConfig.GetSection("AppSettings").Get<AppSettings>();
           
                var result = await signInManager.PasswordSignInAsync(authRequest.userName, authRequest.password, false, lockoutOnFailure: true);
            
                if (result.Succeeded)
                {
                   var userId = await signInManager.UserManager.FindByNameAsync(authRequest.userName);
                
                    loginResponse loginDto = new loginResponse(null,userId.Id,GenerateJWTToken(new UserAccount()));
                    return loginDto;

                }
                else
                {
                    return new loginResponse("Login failed",null, null);
         
                }
            }
            catch (Exception ex)
            {
                return new loginResponse(ex.Message,null,null);
            }
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
