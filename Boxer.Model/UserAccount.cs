using Boxer.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxer.Data
{
    public class UserAccount: IdentityUser
    {
        //public override string UserName { get; set; }
        public DateTime CreatedDate { get; private set; } = DateTime.Now;
        public DateTime ModifiedDate { get; private set; } = DateTime.Now;
        public Guid ModifiedById { get; set; }
        public Guid CreatedById { get; set; }
        
        [NotMapped]
        public String Password { get; set; }
    }
}
