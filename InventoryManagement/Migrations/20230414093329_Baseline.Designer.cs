﻿// <auto-generated />
using System;
using InventoryManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InventoryManagement.Migrations
{
    [DbContext(typeof(InventoryContext))]
    [Migration("20230414093329_Baseline")]
    partial class Baseline
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("InventoryManagement.EntityFrameworkCore.Customer", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("contactNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("email")
                        .HasColumnType("text");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("lastName")
                        .HasColumnType("text");

                    b.Property<int>("paymentType")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("InventoryManagement.EntityFrameworkCore.InventoryItems", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<DateTime>("AddDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("productCategory")
                        .HasColumnType("text");

                    b.Property<string>("productName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("purchasePrice")
                        .HasColumnType("text");

                    b.Property<string>("quantity")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("vendorName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("InventoryItems");
                });

            modelBuilder.Entity("InventoryManagement.EntityFrameworkCore.Order", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<decimal?>("Discount")
                        .HasColumnType("numeric");

                    b.Property<int>("customerId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("orderDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("productId")
                        .HasColumnType("integer");

                    b.Property<int>("quantity")
                        .HasColumnType("integer");

                    b.Property<int>("total_price")
                        .HasColumnType("integer");

                    b.Property<string>("transactionStatus")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("InventoryManagement.EntityFrameworkCore.Product", b =>
                {
                    b.Property<int?>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("id"));

                    b.Property<string>("description")
                        .HasColumnType("text");

                    b.Property<string>("productCategory")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("productName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("productQuantity")
                        .HasColumnType("integer");

                    b.Property<int>("sellingPrice")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("Product");
                });
#pragma warning restore 612, 618
        }
    }
}