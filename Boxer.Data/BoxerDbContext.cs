using Boxer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Boxer.Data
{
    public class BoxerDbContext:DbContext
    {
        public BoxerDbContext(DbContextOptions<BoxerDbContext> options) : base(options) 
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Core Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Core Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<UserAccount>(t=>{
                t.ToTable("UserAccount");
            });


        }
        public DbSet<UserAccount> UserAccount { get; set; }
    }
}
