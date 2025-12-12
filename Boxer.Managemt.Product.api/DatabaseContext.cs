using Boxer.Data;
using Boxer.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Boxer.Managemt.Product.api
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Model.Product>(t => {
                t.ToTable("Product");
            });
            builder.Entity<Category>(t => {
                t.ToTable("Category");
            });


        }
        public DbSet<UserAccount> UserAccount { get; set; }
    }
}
