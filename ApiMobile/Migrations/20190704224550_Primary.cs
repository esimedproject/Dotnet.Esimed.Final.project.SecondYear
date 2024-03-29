﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ApiMobile.Migrations
{
    public partial class Primary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    AuthentificationKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Email = table.Column<string>(nullable: false),
                    Lastname = table.Column<string>(maxLength: 50, nullable: true),
                    Firstname = table.Column<string>(maxLength: 50, nullable: true),
                    Place_of_birth = table.Column<string>(maxLength: 100, nullable: true),
                    Date_of_birth = table.Column<DateTime>(nullable: true),
                    Address = table.Column<string>(maxLength: 150, nullable: true),
                    Phone = table.Column<int>(nullable: true),
                    Password = table.Column<string>(nullable: false),
                    AuthentificationKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Subject = table.Column<string>(maxLength: 50, nullable: false),
                    Means_of_contact = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    UserContactID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_Users_UserContactID",
                        column: x => x.UserContactID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscribes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Start_date_subscribe = table.Column<DateTime>(nullable: false),
                    End_date_subscribe = table.Column<DateTime>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    UserSubscribeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscribes_Users_UserSubscribeID",
                        column: x => x.UserSubscribeID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Magazines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Nb_of_realease = table.Column<int>(nullable: false),
                    Nom = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    date = table.Column<DateTime>(nullable: true),
                    WallpagePATH = table.Column<string>(nullable: true),
                    SubscribesMagazineID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Magazines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Magazines_Subscribes_SubscribesMagazineID",
                        column: x => x.SubscribesMagazineID,
                        principalTable: "Subscribes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    CId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    PaymentAmount = table.Column<double>(nullable: false),
                    transaction = table.Column<long>(nullable: false),
                    MeansOfPayment = table.Column<string>(nullable: true),
                    SubscribesPaymentID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.CId);
                    table.ForeignKey(
                        name: "FK_Payments_Subscribes_SubscribesPaymentID",
                        column: x => x.SubscribesPaymentID,
                        principalTable: "Subscribes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_UserContactID",
                table: "Contacts",
                column: "UserContactID");

            migrationBuilder.CreateIndex(
                name: "IX_Magazines_SubscribesMagazineID",
                table: "Magazines",
                column: "SubscribesMagazineID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SubscribesPaymentID",
                table: "Payments",
                column: "SubscribesPaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribes_UserSubscribeID",
                table: "Subscribes",
                column: "UserSubscribeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Magazines");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Subscribes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
