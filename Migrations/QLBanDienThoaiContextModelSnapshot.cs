﻿// <auto-generated />
using System;
using BTL_QuanLyBanDienThoai.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BTL_QuanLyBanDienThoai.Migrations
{
    [DbContext(typeof(QLBanDienThoaiContext))]
    partial class QLBanDienThoaiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.Attr", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Attr", (string)null);
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.AttributeValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AttrId")
                        .HasColumnType("int");

                    b.Property<int?>("AttributeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("AttrId");

                    b.ToTable("AttributeValue", (string)null);
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.Banner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Banner", (string)null);
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("District")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Ward")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("OrderId")
                        .HasColumnType("int");

                    b.Property<int?>("Price")
                        .HasColumnType("int");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductVariantId")
                        .HasColumnType("int");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.HasIndex("ProductVariantId");

                    b.ToTable("OrderItem", (string)null);
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageDefault")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.ProductAttributeValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AttributeValueId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductVariantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttributeValueId");

                    b.HasIndex("ProductVariantId");

                    b.ToTable("ProductAttributeValue", (string)null);
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.ProductImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImage", (string)null);
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.ProductVariant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductVariant", (string)null);
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.AttributeValue", b =>
                {
                    b.HasOne("BTL_QuanLyBanDienThoai.Models.Attr", "Attr")
                        .WithMany("AttributeValues")
                        .HasForeignKey("AttrId");

                    b.Navigation("Attr");
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.Order", b =>
                {
                    b.HasOne("BTL_QuanLyBanDienThoai.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.OrderItem", b =>
                {
                    b.HasOne("BTL_QuanLyBanDienThoai.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId");

                    b.HasOne("BTL_QuanLyBanDienThoai.Models.Product", "OrderNavigation")
                        .WithMany("OrderItems")
                        .HasForeignKey("ProductId");

                    b.HasOne("BTL_QuanLyBanDienThoai.Models.ProductVariant", "ProductVariant")
                        .WithMany("OrderItems")
                        .HasForeignKey("ProductVariantId");

                    b.Navigation("Order");

                    b.Navigation("OrderNavigation");

                    b.Navigation("ProductVariant");
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.Product", b =>
                {
                    b.HasOne("BTL_QuanLyBanDienThoai.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.ProductAttributeValue", b =>
                {
                    b.HasOne("BTL_QuanLyBanDienThoai.Models.AttributeValue", "AttributeValue")
                        .WithMany("ProductAttributeValues")
                        .HasForeignKey("AttributeValueId");

                    b.HasOne("BTL_QuanLyBanDienThoai.Models.ProductVariant", "ProductVariant")
                        .WithMany("ProductAttributeValues")
                        .HasForeignKey("ProductVariantId");

                    b.Navigation("AttributeValue");

                    b.Navigation("ProductVariant");
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.ProductImage", b =>
                {
                    b.HasOne("BTL_QuanLyBanDienThoai.Models.Product", "Product")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.ProductVariant", b =>
                {
                    b.HasOne("BTL_QuanLyBanDienThoai.Models.Product", "Product")
                        .WithMany("ProductVariants")
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.Attr", b =>
                {
                    b.Navigation("AttributeValues");
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.AttributeValue", b =>
                {
                    b.Navigation("ProductAttributeValues");
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.Product", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("ProductImages");

                    b.Navigation("ProductVariants");
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.ProductVariant", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("ProductAttributeValues");
                });

            modelBuilder.Entity("BTL_QuanLyBanDienThoai.Models.User", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
