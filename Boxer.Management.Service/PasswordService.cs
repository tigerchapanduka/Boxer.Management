using Boxer.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxer.Management.Service
{
    public class PasswordService: IPasswordService
    {
        private readonly IPasswordHasher<UserAccount> _passwordHasher;
        public PasswordService(IPasswordHasher<UserAccount> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string HashPassword(UserAccount user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(UserAccount user, string hashedPassword, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, password);
            // if required, you can handle if result is SuccessRehashNeeded
            return result == PasswordVerificationResult.Success;
        }
    }
}
