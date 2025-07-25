﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pressford.News.Data;

#nullable disable

namespace Pressford.News.Data.Migrations
{
    [DbContext(typeof(PressfordNewsContext))]
    [Migration("20241020005412_Add SP for author")]
    partial class AddSPforauthor
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ArtistCover", b =>
                {
                    b.Property<int>("ArtistsArtistId")
                        .HasColumnType("int");

                    b.Property<int>("CoversCoverId")
                        .HasColumnType("int");

                    b.HasKey("ArtistsArtistId", "CoversCoverId");

                    b.HasIndex("CoversCoverId");

                    b.ToTable("ArtistCover");
                });

            modelBuilder.Entity("Pressford.News.Entities.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Body")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DatePublished")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Article");
                });

            modelBuilder.Entity("Pressford.News.Entities.ArticleLikes", b =>
                {
                    b.Property<int>("LikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LikeId"));

                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LikeId");

                    b.ToTable("ArticleLikes");
                });

            modelBuilder.Entity("Pressford.News.Entities.Artist", b =>
                {
                    b.Property<int>("ArtistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArtistId"));

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ArtistId");

                    b.ToTable("Artist");

                    b.HasData(
                        new
                        {
                            ArtistId = 1,
                            FirstName = "John",
                            LastName = "Doe"
                        },
                        new
                        {
                            ArtistId = 2,
                            FirstName = "Jane",
                            LastName = "Smith"
                        },
                        new
                        {
                            ArtistId = 3,
                            FirstName = "Michael",
                            LastName = "Johnson"
                        });
                });

            modelBuilder.Entity("Pressford.News.Entities.Cover", b =>
                {
                    b.Property<int>("CoverId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CoverId"));

                    b.Property<string>("DesignIdeas")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DigitalOnly")
                        .HasColumnType("bit");

                    b.HasKey("CoverId");

                    b.ToTable("Cover");

                    b.HasData(
                        new
                        {
                            CoverId = 1,
                            DesignIdeas = "Left hand in dark",
                            DigitalOnly = true
                        },
                        new
                        {
                            CoverId = 2,
                            DesignIdeas = "Add a clock",
                            DigitalOnly = true
                        },
                        new
                        {
                            CoverId = 3,
                            DesignIdeas = "Massive Cloud in dark background",
                            DigitalOnly = false
                        });
                });

            modelBuilder.Entity("Pressford.News.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "w.Pressford@pressford.com",
                            FirstName = "W",
                            LastName = "Pressford"
                        },
                        new
                        {
                            Id = 2,
                            Email = "adminUser@pressford.com",
                            FirstName = "Admin",
                            LastName = "User"
                        },
                        new
                        {
                            Id = 3,
                            Email = "normalUser@pressford.com",
                            FirstName = "Normal",
                            LastName = "User"
                        });
                });

            modelBuilder.Entity("Pressford.News.Entities.UserLogin", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Username");

                    b.ToTable("UserLogin");

                    b.HasData(
                        new
                        {
                            Username = "w.Pressford@pressford.com",
                            Password = "admin",
                            Role = "Publisher"
                        },
                        new
                        {
                            Username = "adminUser@pressford.com",
                            Password = "admin",
                            Role = "Publisher"
                        },
                        new
                        {
                            Username = "normalUser@pressford.com",
                            Password = "user",
                            Role = "User"
                        });
                });

            modelBuilder.Entity("ArtistCover", b =>
                {
                    b.HasOne("Pressford.News.Entities.Artist", null)
                        .WithMany()
                        .HasForeignKey("ArtistsArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pressford.News.Entities.Cover", null)
                        .WithMany()
                        .HasForeignKey("CoversCoverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Pressford.News.Entities.UserLogin", b =>
                {
                    b.HasOne("Pressford.News.Entities.User", "User")
                        .WithOne("LoginInfo")
                        .HasForeignKey("Pressford.News.Entities.UserLogin", "Username")
                        .HasPrincipalKey("Pressford.News.Entities.User", "Email")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pressford.News.Entities.User", b =>
                {
                    b.Navigation("LoginInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
