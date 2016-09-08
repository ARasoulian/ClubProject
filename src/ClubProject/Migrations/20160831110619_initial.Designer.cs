using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ClubProject.Models;

namespace ClubProject.Migrations
{
    [DbContext(typeof(ClubContext))]
    [Migration("20160831110619_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ClubProject.Models.Club", b =>
                {
                    b.Property<int>("ClubId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .HasAnnotation("MaxLength", 150);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.HasKey("ClubId");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("ClubProject.Models.Picture", b =>
                {
                    b.Property<int>("PictureId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClubId");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsCoverPicture");

                    b.Property<int>("Order");

                    b.Property<string>("PictureName")
                        .IsRequired();

                    b.HasKey("PictureId");

                    b.HasIndex("ClubId");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("ClubProject.Models.Picture", b =>
                {
                    b.HasOne("ClubProject.Models.Club", "Club")
                        .WithMany("Pictures")
                        .HasForeignKey("ClubId");
                });
        }
    }
}
