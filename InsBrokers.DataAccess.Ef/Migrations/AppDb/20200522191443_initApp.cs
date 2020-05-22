using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InsBrokers.DataAccess.Ef.Migrations.AppDb
{
    public partial class initApp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Base");

            migrationBuilder.EnsureSchema(
                name: "Insurance");

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Base",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    MobileNumber = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    MustChangePassword = table.Column<bool>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    LastLoginDateMi = table.Column<DateTime>(nullable: true),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    LastLoginDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    BirthDay = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    NationalCode = table.Column<string>(type: "char(10)", maxLength: 10, nullable: false),
                    Password = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    NewPassword = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 25, nullable: false),
                    Family = table.Column<string>(maxLength: 30, nullable: false),
                    FatherName = table.Column<string>(maxLength: 25, nullable: false),
                    BaseInsurance = table.Column<string>(maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "Base",
                columns: table => new
                {
                    AddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Province = table.Column<string>(maxLength: 250, nullable: false),
                    City = table.Column<string>(maxLength: 250, nullable: false),
                    AddressDetails = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Address_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BankAccount",
                schema: "Base",
                columns: table => new
                {
                    BankAccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    BankName = table.Column<byte>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    ModifyDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    ModifyDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    AccountNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Shaba = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccount", x => x.BankAccountId);
                    table.ForeignKey(
                        name: "FK_BankAccount_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAttachment",
                schema: "Base",
                columns: table => new
                {
                    UserAttachmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    FileType = table.Column<byte>(nullable: false),
                    UserAttachmentType = table.Column<int>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Extention = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    Url = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAttachment", x => x.UserAttachmentId);
                    table.ForeignKey(
                        name: "FK_UserAttachment_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Loss",
                schema: "Insurance",
                columns: table => new
                {
                    LossId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    Cost = table.Column<int>(nullable: false),
                    LossType = table.Column<string>(nullable: false),
                    LossDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    ModifyDateMi = table.Column<DateTime>(nullable: false),
                    LossDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    ModifyDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    RelationType = table.Column<string>(maxLength: 25, nullable: false),
                    PatientName = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loss", x => x.LossId);
                    table.ForeignKey(
                        name: "FK_Loss_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_UserId",
                schema: "Base",
                table: "Address",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_UserId",
                schema: "Base",
                table: "BankAccount",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileNumber",
                schema: "Base",
                table: "User",
                column: "MobileNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAttachment_UserId",
                schema: "Base",
                table: "UserAttachment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Loss_UserId",
                schema: "Insurance",
                table: "Loss",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "BankAccount",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "UserAttachment",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "Loss",
                schema: "Insurance");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Base");
        }
    }
}
