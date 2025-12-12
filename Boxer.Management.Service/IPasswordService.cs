using Boxer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxer.Management.Service
{
    public interface IPasswordService
    {
        string HashPassword(UserAccount user, string password);
        bool VerifyPassword(UserAccount user, string hashedPassword, string password);
    }
}
