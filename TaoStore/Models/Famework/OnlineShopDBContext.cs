namespace Models.Famework
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OnlineShopDBContext : DbContext
    {
        public OnlineShopDBContext()
            : base("name=OnlineShopDBContext")
        {
        }

        public virtual DbSet<Acount> Acounts { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<CartDetail> CartDetails { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductSeller> ProductSellers { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Acount>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Acount>()
                .Property(e => e.Avatar)
                .IsUnicode(false);

            modelBuilder.Entity<Acount>()
                .Property(e => e.PassWord)
                .IsUnicode(false);

            modelBuilder.Entity<Cart>()
                .HasMany(e => e.CartDetails)
                .WithRequired(e => e.Cart)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.CartDetails)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasOptional(e => e.ProductSeller)
                .WithRequired(e => e.Product);

            modelBuilder.Entity<Size>()
                .Property(e => e.TextSize)
                .IsUnicode(false);
        }
    }
}
