using Microsoft.EntityFrameworkCore;
using BTL_QuanLyBanDienThoai.Models;

namespace BTL_QuanLyBanDienThoai.Data
{
    public class QLBanDienThoaiContext : DbContext
    {
        public QLBanDienThoaiContext(DbContextOptions<QLBanDienThoaiContext> options) : base(options) { }
        public virtual DbSet<Attr> Attrs { get; set; }
        public virtual DbSet<AttributeValue> AttributeValues { get; set; }
        public virtual DbSet<Banner> Banners { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<ProductVariant> ProductVariants { get; set; }
        public virtual DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attr>().ToTable(nameof(Attr));
            modelBuilder.Entity<AttributeValue>().ToTable(nameof(AttributeValue));
            modelBuilder.Entity<Banner>().ToTable(nameof(Banner));
            modelBuilder.Entity<Category>().ToTable(nameof(Category));
            modelBuilder.Entity<Order>().ToTable(nameof(Order));
            modelBuilder.Entity<OrderItem>().ToTable(nameof(OrderItem));
            modelBuilder.Entity<Product>().ToTable(nameof(Product));
            modelBuilder.Entity<ProductAttributeValue>().ToTable(nameof(ProductAttributeValue));
            modelBuilder.Entity<ProductImage>().ToTable(nameof(ProductImage));
            modelBuilder.Entity<ProductVariant>().ToTable(nameof(ProductVariant));
            modelBuilder.Entity<User>().ToTable(nameof(User));
        }
    }
}
