﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tickets.API.Data;

#nullable disable

namespace Tickets.API.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    [Migration("20231001195802_Migracion inicial Auth")]
    partial class MigracioninicialAuth
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "43868f73-fc0a-4fb6-9ded-5646ae843552",
                            ConcurrencyStamp = "43868f73-fc0a-4fb6-9ded-5646ae843552",
                            Name = "Administrador",
                            NormalizedName = "ADMINISTRADOR"
                        },
                        new
                        {
                            Id = "caf48194-53ee-4903-b9b9-8642a8505a71",
                            ConcurrencyStamp = "caf48194-53ee-4903-b9b9-8642a8505a71",
                            Name = "Cliente",
                            NormalizedName = "CLIENTE"
                        },
                        new
                        {
                            Id = "228388ef-e006-4ae8-8705-95f6d598c26f",
                            ConcurrencyStamp = "228388ef-e006-4ae8-8705-95f6d598c26f",
                            Name = "Agente",
                            NormalizedName = "AGENTE"
                        },
                        new
                        {
                            Id = "61ca7686-7128-4bfe-82a3-a27f204f91d9",
                            ConcurrencyStamp = "61ca7686-7128-4bfe-82a3-a27f204f91d9",
                            Name = "Supervisor",
                            NormalizedName = "SUPERVISOR"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "c6e69bd3-a1a9-46d7-b00f-05900d59a585",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "c9e88a1e-e60c-4e2f-a826-3b27e1055da6",
                            Email = "admin@imss.gob.mx",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@IMSS.GOB.MX",
                            NormalizedUserName = "ADMIN@IMSS.GOB.MX",
                            PasswordHash = "AQAAAAIAAYagAAAAEHdwxe9BfFHfkq7A+qZc7kAi4r2b65Z6/VTwvaN3FO/dDd3rmenQlapUj+LBaK2owg==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "5ead0c6b-22dd-4717-8cbe-baa0698221fd",
                            TwoFactorEnabled = false,
                            UserName = "admin@imss.gob.mx"
                        },
                        new
                        {
                            Id = "20eeea3c-4b7c-419c-a457-569492ab7677",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "501d6306-33f3-43fa-a774-bb4ee77d2fcd",
                            Email = "cliente@imss.gob.mx",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "CLIENTE@IMSS.GOB.MX",
                            NormalizedUserName = "CLIENTE@IMSS.GOB.MX",
                            PasswordHash = "AQAAAAIAAYagAAAAEKnpYutMqBYoSg5MH+msRICieBfD1/S/xFClFLD8XkW4TI5L/4pm2Xr6Ispswz37+A==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "ea85fd84-e892-4401-89d0-24285ccc6849",
                            TwoFactorEnabled = false,
                            UserName = "cliente@imss.gob.mx"
                        },
                        new
                        {
                            Id = "2f45b379-9a6e-4291-a07f-81afeb02a05c",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "e1ee50c9-3a2b-4fb9-904e-52b11a70ac5a",
                            Email = "agente@imss.gob.mx",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "AGENTE@IMSS.GOB.MX",
                            NormalizedUserName = "AGENTE@IMSS.GOB.MX",
                            PasswordHash = "AQAAAAIAAYagAAAAEKO7YqYjLI3N5Az9RR9qFx2mcWrBwhpv8ERRyTooHK3ayW/kC4/DTuB/xrVEHqdQiw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "f7579d8e-82ea-41c1-8e01-bf61f3d49d51",
                            TwoFactorEnabled = false,
                            UserName = "agente@imss.gob.mx"
                        },
                        new
                        {
                            Id = "3eb1755e-5e01-4480-a20e-621b689fa1bc",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "fe91e962-fe4b-4df2-a43f-3b3e2942ce6b",
                            Email = "supervisor@imss.gob.mx",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "SUPERVISOR@IMSS.GOB.MX",
                            NormalizedUserName = "SUPERVISOR@IMSS.GOB.MX",
                            PasswordHash = "AQAAAAIAAYagAAAAEJROLi/z9D/E/LmyLowu31+khk6xioH+LHHYtQoooY1CZkMnhTV12//fZn/Po1MNeQ==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "9ec9cb01-05ee-4381-a424-ef19a05cdeec",
                            TwoFactorEnabled = false,
                            UserName = "supervisor@imss.gob.mx"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "c6e69bd3-a1a9-46d7-b00f-05900d59a585",
                            RoleId = "43868f73-fc0a-4fb6-9ded-5646ae843552"
                        },
                        new
                        {
                            UserId = "20eeea3c-4b7c-419c-a457-569492ab7677",
                            RoleId = "caf48194-53ee-4903-b9b9-8642a8505a71"
                        },
                        new
                        {
                            UserId = "2f45b379-9a6e-4291-a07f-81afeb02a05c",
                            RoleId = "228388ef-e006-4ae8-8705-95f6d598c26f"
                        },
                        new
                        {
                            UserId = "3eb1755e-5e01-4480-a20e-621b689fa1bc",
                            RoleId = "61ca7686-7128-4bfe-82a3-a27f204f91d9"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
