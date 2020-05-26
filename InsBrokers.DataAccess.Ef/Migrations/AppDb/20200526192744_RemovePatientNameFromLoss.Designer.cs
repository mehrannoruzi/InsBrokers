﻿// <auto-generated />
using System;
using InsBrokers.DataAccess.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InsBrokers.DataAccess.Ef.Migrations.AppDb
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200526192744_RemovePatientNameFromLoss")]
    partial class RemovePatientNameFromLoss
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InsBrokers.Domain.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddressDetails")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Address","Base");
                });

            modelBuilder.Entity("InsBrokers.Domain.BankAccount", b =>
                {
                    b.Property<int>("BankAccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountNumber")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<byte>("BankName")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifyDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifyDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Shaba")
                        .HasColumnType("varchar(24)")
                        .HasMaxLength(24);

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BankAccountId");

                    b.HasIndex("UserId");

                    b.ToTable("BankAccount","Base");
                });

            modelBuilder.Entity("InsBrokers.Domain.Loss", b =>
                {
                    b.Property<int>("LossId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<DateTime>("LossDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("LossDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<string>("LossType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifyDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifyDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<int?>("RelativeId")
                        .HasColumnType("int");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LossId");

                    b.HasIndex("UserId");

                    b.ToTable("Loss","Insurance");
                });

            modelBuilder.Entity("InsBrokers.Domain.LossAsset", b =>
                {
                    b.Property<int>("LossAssetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Extention")
                        .IsRequired()
                        .HasColumnType("varchar(5)")
                        .HasMaxLength(5);

                    b.Property<byte>("FileType")
                        .HasColumnType("tinyint");

                    b.Property<string>("FileUrl")
                        .HasColumnType("varchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<int>("LossId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(35)")
                        .HasMaxLength(35);

                    b.Property<string>("PhysicalPath")
                        .HasColumnType("varchar(1000)")
                        .HasMaxLength(1000);

                    b.HasKey("LossAssetId");

                    b.HasIndex("LossId");

                    b.ToTable("LossAsset","Insurance");
                });

            modelBuilder.Entity("InsBrokers.Domain.Relative", b =>
                {
                    b.Property<int>("RelativeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BirthDay")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<DateTime>("BirthDayMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("Family")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("FatherName")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.Property<byte>("Gender")
                        .HasColumnType("tinyint");

                    b.Property<string>("IdentityNumber")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasMaxLength(10);

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.Property<string>("NationalCode")
                        .IsRequired()
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<byte>("RelativeType")
                        .HasColumnType("tinyint");

                    b.Property<byte>("TakafolKind")
                        .HasColumnType("tinyint");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RelativeId");

                    b.HasIndex("UserId");

                    b.ToTable("Relative","Base");
                });

            modelBuilder.Entity("InsBrokers.Domain.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("BaseInsurance")
                        .HasColumnType("tinyint");

                    b.Property<string>("BirthDay")
                        .IsRequired()
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<DateTime>("BirthDayMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Family")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("FatherName")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.Property<byte>("Gender")
                        .HasColumnType("tinyint");

                    b.Property<string>("IdentityNumber")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasMaxLength(10);

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastLoginDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<long>("MobileNumber")
                        .HasColumnType("bigint");

                    b.Property<bool>("MustChangePassword")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.Property<string>("NationalCode")
                        .IsRequired()
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<string>("NewPassword")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("UserId");

                    b.HasIndex("MobileNumber")
                        .IsUnique()
                        .HasName("IX_MobileNumber");

                    b.ToTable("User","Base");
                });

            modelBuilder.Entity("InsBrokers.Domain.UserAttachment", b =>
                {
                    b.Property<int>("UserAttachmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Extention")
                        .IsRequired()
                        .HasColumnType("varchar(5)")
                        .HasMaxLength(5);

                    b.Property<byte>("FileType")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("InsertDateMi")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsertDateSh")
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(16)")
                        .HasMaxLength(16);

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasMaxLength(150);

                    b.Property<int>("UserAttachmentType")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserAttachmentId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAttachment","Base");
                });

            modelBuilder.Entity("InsBrokers.Domain.Address", b =>
                {
                    b.HasOne("InsBrokers.Domain.User", "User")
                        .WithMany("Addresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("InsBrokers.Domain.BankAccount", b =>
                {
                    b.HasOne("InsBrokers.Domain.User", "User")
                        .WithMany("BankAccounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("InsBrokers.Domain.Loss", b =>
                {
                    b.HasOne("InsBrokers.Domain.User", "User")
                        .WithMany("losses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("InsBrokers.Domain.LossAsset", b =>
                {
                    b.HasOne("InsBrokers.Domain.Loss", "Loss")
                        .WithMany("LossAssets")
                        .HasForeignKey("LossId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("InsBrokers.Domain.Relative", b =>
                {
                    b.HasOne("InsBrokers.Domain.User", "User")
                        .WithMany("Relatives")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("InsBrokers.Domain.UserAttachment", b =>
                {
                    b.HasOne("InsBrokers.Domain.User", "User")
                        .WithMany("UserAttachments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
