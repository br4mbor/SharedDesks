﻿// <auto-generated />
using System;
using Abb.Euopc.SharedDesks.EF.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Abb.Euopc.SharedDesks.EF.Migrations
{
    [DbContext(typeof(SharedDesksContext))]
    [Migration("20230101233458_AreaImage")]
    partial class AreaImage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Abb.Euopc.SharedDesks.Domain.Entities.Area", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Floor")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("Abb.Euopc.SharedDesks.Domain.Entities.Desk", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AreaId")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.ToTable("Desks");
                });

            modelBuilder.Entity("Abb.Euopc.SharedDesks.Domain.Entities.DeskItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("DeskItems");
                });

            modelBuilder.Entity("Abb.Euopc.SharedDesks.Domain.Entities.DeskItemToDesk", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DeskId")
                        .HasColumnType("int");

                    b.Property<int>("DeskItemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeskId");

                    b.HasIndex("DeskItemId");

                    b.ToTable("DeskItemToDesk");
                });

            modelBuilder.Entity("Abb.Euopc.SharedDesks.Domain.Entities.DeskItemType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("DeskItemTypes");
                });

            modelBuilder.Entity("Abb.Euopc.SharedDesks.Domain.Entities.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CreatedByEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeskId")
                        .HasColumnType("int");

                    b.Property<string>("ReservedForEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("DeskId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Abb.Euopc.SharedDesks.Domain.Entities.Desk", b =>
                {
                    b.HasOne("Abb.Euopc.SharedDesks.Domain.Entities.Area", "Area")
                        .WithMany()
                        .HasForeignKey("AreaId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Area");
                });

            modelBuilder.Entity("Abb.Euopc.SharedDesks.Domain.Entities.DeskItem", b =>
                {
                    b.HasOne("Abb.Euopc.SharedDesks.Domain.Entities.DeskItemType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Type");
                });

            modelBuilder.Entity("Abb.Euopc.SharedDesks.Domain.Entities.DeskItemToDesk", b =>
                {
                    b.HasOne("Abb.Euopc.SharedDesks.Domain.Entities.Desk", "Desk")
                        .WithMany("DeskItemsToDesks")
                        .HasForeignKey("DeskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Abb.Euopc.SharedDesks.Domain.Entities.DeskItem", "DeskItem")
                        .WithMany()
                        .HasForeignKey("DeskItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Desk");

                    b.Navigation("DeskItem");
                });

            modelBuilder.Entity("Abb.Euopc.SharedDesks.Domain.Entities.Reservation", b =>
                {
                    b.HasOne("Abb.Euopc.SharedDesks.Domain.Entities.Desk", "Desk")
                        .WithMany()
                        .HasForeignKey("DeskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Desk");
                });

            modelBuilder.Entity("Abb.Euopc.SharedDesks.Domain.Entities.Desk", b =>
                {
                    b.Navigation("DeskItemsToDesks");
                });
#pragma warning restore 612, 618
        }
    }
}
