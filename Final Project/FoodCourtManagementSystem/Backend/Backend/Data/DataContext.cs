using BackendUsers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BackendUsers.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<OrderProduct>()
            //   //.HasOne(op => op.Order)
            //   .WithMany(o => o.Products_of_this_order)
            //   .HasForeignKey(op => op.OrderId);

            //modelBuilder.Entity<ProductDetails>()
            //    .HasOne(p => p.Product)
            //    .WithMany(p=> p.ProductQuantity)
            //    .HasForeignKey(p => p.ProductId);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet <User> Users { get; set; }
        public DbSet <Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        //public class YourDbContextFactory : IDesignTimeDbContextFactory<DataContext>
        //{

        //    public DataContext CreateDbContext(string[] args)

        //    {

        //        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

        //        optionsBuilder.UseSqlServer("server=LAP-PF4PQ4PS\\DEMO;database=Users;trusted_connection=true;TrustServerCertificate=true;user id=sa;password=Sql@12345678900");

        //        return new DataContext(optionsBuilder.Options);

        //    }

        //}


    }
}
