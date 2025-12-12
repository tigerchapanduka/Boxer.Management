using Boxer.Data;
using Boxer.Management.Product.Data;
using Boxer.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Boxer.Model.Dto;

namespace Boxer.Management.Service
{
    public interface IUserAccountService
    {
        public Task<UserAccountResponse> Save(UserAccountRequest userAccountRequest, DatabaseContext dbcontext);
        public Task<loginDto> LoginUserAccount(UserAccount authRequest, DatabaseContext dbcontext);
    }
}
