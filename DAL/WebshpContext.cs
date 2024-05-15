using Microsoft.EntityFrameworkCore;
using Model;
using System.Collections.Generic;

namespace DAL {
    public class WebshpContext : DbContext
    {
        public WebshpContext(DbContextOptions<WebshpContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<BasketPosition> BasketPositions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderPosition> OrderPositions { get; set; }

      
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
            .HasOne(p => p.Group)
            .WithMany()
            .HasForeignKey(p => p.GroupID)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductGroup>()
       .HasOne(pg => pg.PGroup)
       .WithMany()
       .HasForeignKey(pg=>pg.PGroupID)
       .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BasketPosition>()
                .HasOne(bp => bp.User)
                .WithMany()
                .HasForeignKey(bp => bp.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderPosition>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderPositions)
                .HasForeignKey(op => op.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderPosition>()
            .HasOne(op => op.Product)
            .WithMany()
            .HasForeignKey(op => op.ProductID)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserGroup)
                .WithMany(ug => ug.Users)
                .HasForeignKey(u => u.GroupID)
                .OnDelete(DeleteBehavior.Cascade);

        }

    
}


}
