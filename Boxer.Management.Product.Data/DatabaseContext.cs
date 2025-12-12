using Boxer.Data;
using Boxer.Model;

using Microsoft.EntityFrameworkCore;

namespace Boxer.Management.Product.Data
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
        public DbSet<Model.Product> product { get; set; }
        public DbSet<Category> category { get; set; }

    }
}
