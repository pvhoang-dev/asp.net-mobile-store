using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BTL_QuanLyBanDienThoai.Areas.Admin.Models;

public partial class BaiTapLonLapTrinhWebContext : DbContext
{
    public BaiTapLonLapTrinhWebContext()
    {
    }

    public BaiTapLonLapTrinhWebContext(DbContextOptions<BaiTapLonLapTrinhWebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attribute> Attributes { get; set; }

    public virtual DbSet<AttributeValue> AttributeValues { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }

    public virtual DbSet<ProductVariant> ProductVariants { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-LEGION\\SQLEXPRESS;Initial Catalog=BAI_TAP_LON_LAP_TRINH_WEB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attribute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attribut__3214EC07F2637C95");

            entity.ToTable("Attribute");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<AttributeValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attribut__3214EC07992569C6");

            entity.ToTable("AttributeValue");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AttributeId).HasColumnName("Attribute_Id");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Attribute).WithMany(p => p.AttributeValues)
                .HasForeignKey(d => d.AttributeId)
                .HasConstraintName("FK__Attribute__Attri__571DF1D5");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07DF35917F");

            entity.ToTable("Category");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC074662CD6A");

            entity.ToTable("Product");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CategoryId).HasColumnName("Category_Id");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.ImageDefault)
                .HasMaxLength(100)
                .HasColumnName("Image_Default");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price1).HasColumnName("Price_1");
            entity.Property(e => e.Price2).HasColumnName("Price_2");
            entity.Property(e => e.Slug).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Product__Categor__534D60F1");
        });

        modelBuilder.Entity<ProductAttributeValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductA__3214EC0714039CDC");

            entity.ToTable("ProductAttributeValue");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AttributeValueId).HasColumnName("Attribute_Value_Id");
            entity.Property(e => e.ProductId).HasColumnName("Product_Id");
            entity.Property(e => e.ProductVariantId).HasColumnName("Product_Variant_Id");

            entity.HasOne(d => d.AttributeValue).WithMany(p => p.ProductAttributeValues)
                .HasForeignKey(d => d.AttributeValueId)
                .HasConstraintName("FK__ProductAt__Attri__5629CD9C");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.ProductAttributeValues)
                .HasForeignKey(d => d.ProductVariantId)
                .HasConstraintName("FK__ProductAt__Produ__5535A963");
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductV__3214EC07E7607D5C");

            entity.ToTable("ProductVariant");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ProductId).HasColumnName("Product_Id");
            entity.Property(e => e.Slug).HasMaxLength(100);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductVa__Produ__5441852A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
