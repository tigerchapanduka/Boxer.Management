using Boxer.Data;
using Boxer.Management.Security.Authentication.Data;
using Boxer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Boxer.Model.Dto;


namespace Boxer.Management.Security.Authentication.Service
{
    public interface IUserAccountService
    {
        public Task<UserAccountResponse> Save(UserAccountRequest userAccountRequest, DatabaseContext dbcontext, UserManager<UserAccount> userManager);
        public Task<loginResponse> LoginUserAccount(AuthenticationRequest authRequest, DatabaseContext dbcontext, SignInManager<UserAccount> signInManager);
    }
}
