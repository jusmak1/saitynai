﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SocialAPI.Models;

namespace SocialAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20200918193128_comments")]
    partial class comments
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SocialAPI.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorEmail")
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PostedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorEmail");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("SocialAPI.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("PostedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PostedBy");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("SocialAPI.Models.Role", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Name");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Name = "Common"
                        },
                        new
                        {
                            Name = "Administrator"
                        });
                });

            modelBuilder.Entity("SocialAPI.Models.User", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SocialAPI.Models.UserRoles", b =>
                {
                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserEmail", "RoleName");

                    b.HasIndex("RoleName");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("SocialAPI.Models.Comment", b =>
                {
                    b.HasOne("SocialAPI.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorEmail");

                    b.HasOne("SocialAPI.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SocialAPI.Models.Post", b =>
                {
                    b.HasOne("SocialAPI.Models.User", "Creator")
                        .WithMany("Posts")
                        .HasForeignKey("PostedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SocialAPI.Models.UserRoles", b =>
                {
                    b.HasOne("SocialAPI.Models.Role", "Role")
                        .WithMany("UsersRole")
                        .HasForeignKey("RoleName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SocialAPI.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
