﻿// <auto-generated />
using System;
using ApiMobile.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ApiMobile.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ApiMobile.Models.Admins", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthentificationKey");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("ApiMobile.Models.Contacts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Means_of_contact");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("UserContactID");

                    b.HasKey("Id");

                    b.HasIndex("UserContactID");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("ApiMobile.Models.Magazines", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("Nb_of_realease");

                    b.Property<string>("Nom");

                    b.Property<double>("Price");

                    b.Property<int?>("SubscribesMagazineID");

                    b.Property<string>("WallpagePATH");

                    b.Property<DateTime?>("date");

                    b.HasKey("Id");

                    b.HasIndex("SubscribesMagazineID");

                    b.ToTable("Magazines");
                });

            modelBuilder.Entity("ApiMobile.Models.Payments", b =>
                {
                    b.Property<int>("CId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MeansOfPayment");

                    b.Property<double>("PaymentAmount");

                    b.Property<int?>("SubscribesPaymentID");

                    b.Property<long>("transaction");

                    b.HasKey("CId");

                    b.HasIndex("SubscribesPaymentID");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("ApiMobile.Models.Subscribes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("End_date_subscribe")
                        .IsRequired();

                    b.Property<DateTime?>("Start_date_subscribe")
                        .IsRequired();

                    b.Property<bool>("Status");

                    b.Property<int?>("UserSubscribeID");

                    b.HasKey("Id");

                    b.HasIndex("UserSubscribeID");

                    b.ToTable("Subscribes");
                });

            modelBuilder.Entity("ApiMobile.Models.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .HasMaxLength(150);

                    b.Property<string>("AuthentificationKey");

                    b.Property<DateTime?>("Date_of_birth");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Firstname")
                        .HasMaxLength(50);

                    b.Property<string>("Lastname")
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int?>("Phone");

                    b.Property<string>("Place_of_birth")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ApiMobile.Models.Contacts", b =>
                {
                    b.HasOne("ApiMobile.Models.Users", "user")
                        .WithMany("UsersContact")
                        .HasForeignKey("UserContactID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ApiMobile.Models.Magazines", b =>
                {
                    b.HasOne("ApiMobile.Models.Subscribes", "Subscribe")
                        .WithMany("SubscribeMagazine")
                        .HasForeignKey("SubscribesMagazineID");
                });

            modelBuilder.Entity("ApiMobile.Models.Payments", b =>
                {
                    b.HasOne("ApiMobile.Models.Subscribes", "subscribe")
                        .WithMany("SubscribesPayment")
                        .HasForeignKey("SubscribesPaymentID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ApiMobile.Models.Subscribes", b =>
                {
                    b.HasOne("ApiMobile.Models.Users", "user")
                        .WithMany("UsersSubscribe")
                        .HasForeignKey("UserSubscribeID");
                });
#pragma warning restore 612, 618
        }
    }
}
