﻿// <auto-generated />
using System;
using FotokopiRandevuAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FotokopiRandevuAPI.Persistence.Migrations
{
    [DbContext(typeof(fotokopiRandevuAPIDbContext))]
    [Migration("20241010203418_mig_1")]
    partial class mig_1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Identity.AppRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "a55c5f9f-4f8c-4848-882f-0bcb3ec62171",
                            ConcurrencyStamp = "1161f82a-07be-41fd-9c8d-67cc857b3559",
                            Name = "admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "128f0e53-f259-411a-b4be-e050e48c199e",
                            ConcurrencyStamp = "e3077933-8bac-4b5b-85ad-263678371ea1",
                            Name = "customer",
                            NormalizedName = "CUSTOMER"
                        },
                        new
                        {
                            Id = "82a8f83f-47b0-468b-9ddf-cc450d84f4ea",
                            ConcurrencyStamp = "0581518f-55c9-4c4e-ba99-dc5df747b55a",
                            Name = "agency",
                            NormalizedName = "AGENCY"
                        });
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Identity.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("RefreshTokenEndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("AppUser");

                    b.UseTphMappingStrategy();

                    b.HasData(
                        new
                        {
                            Id = "c5bc8bb5-0f4f-452a-911c-9844f7e2aac7",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "f0f39cd3-b32d-400f-b1bb-f8942ef84bdb",
                            Email = "admin@gmail.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            Name = "admin",
                            NormalizedEmail = "ADMIN@GMAIL.COM",
                            NormalizedUserName = "ADMIN",
                            PasswordHash = "AQAAAAIAAYagAAAAEPqLpXiNQV10n6+eoIUes28yv1pnmtZydBCBn4InQnndNShR9HKHgxnDQ9cWJdx++A==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "20505a6b-97cc-4eff-aa64-ac631c4dcb8b",
                            Surname = "admin",
                            TwoFactorEnabled = false,
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Identity.Extra.BeAnAgencyRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AgencyId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AgencyName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CustomerId")
                        .HasColumnType("text");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("BeAnAgencyRequests");
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Order.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AgencyId")
                        .HasColumnType("text");

                    b.Property<string>("CommentText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CustomerId")
                        .HasColumnType("text");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<float>("StarRating")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("AgencyId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Order.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AgencyId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CustomerId")
                        .HasColumnType("text");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("AgencyId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Products.AgencyProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AgencyId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AgencyId");

                    b.HasIndex("ProductId");

                    b.ToTable("AgencyProducts");
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Products.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("ColorOption")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PaperType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PrintType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.ToTable("IdentityUserLogin");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUserRole<string>");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Identity.Extra.Agency", b =>
                {
                    b.HasBaseType("FotokopiRandevuAPI.Domain.Entities.Identity.AppUser");

                    b.Property<string>("AgencyBio")
                        .HasColumnType("text");

                    b.Property<string>("AgencyName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsConfirmedAgency")
                        .HasColumnType("boolean");

                    b.HasDiscriminator().HasValue("Agency");
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Identity.Extra.Customer", b =>
                {
                    b.HasBaseType("FotokopiRandevuAPI.Domain.Entities.Identity.AppUser");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Identity.AppUserRole", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUserRole<string>");

                    b.HasDiscriminator().HasValue("AppUserRole");

                    b.HasData(
                        new
                        {
                            UserId = "c5bc8bb5-0f4f-452a-911c-9844f7e2aac7",
                            RoleId = "a55c5f9f-4f8c-4848-882f-0bcb3ec62171"
                        });
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Identity.Extra.BeAnAgencyRequest", b =>
                {
                    b.HasOne("FotokopiRandevuAPI.Domain.Entities.Identity.AppUser", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.OwnsOne("FotokopiRandevuAPI.Domain.Entities.Identity.Extra.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("BeAnAgencyRequestId")
                                .HasColumnType("uuid");

                            b1.Property<string>("District")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("District");

                            b1.Property<string>("Extra")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("Extra");

                            b1.Property<string>("Province")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("Province");

                            b1.HasKey("BeAnAgencyRequestId");

                            b1.ToTable("BeAnAgencyRequests");

                            b1.WithOwner()
                                .HasForeignKey("BeAnAgencyRequestId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Order.Comment", b =>
                {
                    b.HasOne("FotokopiRandevuAPI.Domain.Entities.Identity.Extra.Agency", "Agency")
                        .WithMany("Comments")
                        .HasForeignKey("AgencyId");

                    b.HasOne("FotokopiRandevuAPI.Domain.Entities.Identity.Extra.Customer", "Customer")
                        .WithMany("Comments")
                        .HasForeignKey("CustomerId");

                    b.HasOne("FotokopiRandevuAPI.Domain.Entities.Order.Order", "Order")
                        .WithOne("Comment")
                        .HasForeignKey("FotokopiRandevuAPI.Domain.Entities.Order.Comment", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Agency");

                    b.Navigation("Customer");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Order.Order", b =>
                {
                    b.HasOne("FotokopiRandevuAPI.Domain.Entities.Identity.Extra.Agency", "Agency")
                        .WithMany("Orders")
                        .HasForeignKey("AgencyId");

                    b.HasOne("FotokopiRandevuAPI.Domain.Entities.Identity.Extra.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId");

                    b.Navigation("Agency");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Products.AgencyProduct", b =>
                {
                    b.HasOne("FotokopiRandevuAPI.Domain.Entities.Identity.Extra.Agency", "Agency")
                        .WithMany("AgencyProducts")
                        .HasForeignKey("AgencyId");

                    b.HasOne("FotokopiRandevuAPI.Domain.Entities.Products.Product", "Product")
                        .WithMany("AgencyProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Agency");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("FotokopiRandevuAPI.Domain.Entities.Identity.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("FotokopiRandevuAPI.Domain.Entities.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FotokopiRandevuAPI.Domain.Entities.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("FotokopiRandevuAPI.Domain.Entities.Identity.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FotokopiRandevuAPI.Domain.Entities.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("FotokopiRandevuAPI.Domain.Entities.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Identity.Extra.Agency", b =>
                {
                    b.OwnsOne("FotokopiRandevuAPI.Domain.Entities.Identity.Extra.Address", "Address", b1 =>
                        {
                            b1.Property<string>("AgencyId")
                                .HasColumnType("text");

                            b1.Property<string>("District")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("District");

                            b1.Property<string>("Extra")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("Extra");

                            b1.Property<string>("Province")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("Province");

                            b1.HasKey("AgencyId");

                            b1.ToTable("AspNetUsers");

                            b1.WithOwner()
                                .HasForeignKey("AgencyId");
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Order.Order", b =>
                {
                    b.Navigation("Comment")
                        .IsRequired();
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Products.Product", b =>
                {
                    b.Navigation("AgencyProducts");
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Identity.Extra.Agency", b =>
                {
                    b.Navigation("AgencyProducts");

                    b.Navigation("Comments");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("FotokopiRandevuAPI.Domain.Entities.Identity.Extra.Customer", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
