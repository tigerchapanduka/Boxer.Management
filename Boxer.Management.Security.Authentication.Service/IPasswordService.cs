using Boxer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxer.Management.Security.Authentication.Service
{
    public interface IPasswordService
    {
       public string HashPassword(UserAccount user, string password);
       public  bool VerifyPassword(UserAccount user, string hashedPassword, string password);
    }
}
