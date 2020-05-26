using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InsBrokers.DataAccess.Ef.Migrations.AppDb
{
    public partial class AddRelativeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BirthDay",
                schema: "Base",
                table: "User",
                type: "char(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDayMi",
                schema: "Base",
                table: "User",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte>(
                name: "Gender",
                schema: "Base",
                table: "User",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                schema: "Base",
                table: "User",
                type: "varchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Relative",
                schema: "Base",
                columns: table => new
                {
                    RelativeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    Gender = table.Column<byte>(nullable: false),
                    TakafolKind = table.Column<byte>(nullable: false),
                    RelativeType = table.Column<byte>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    BirthDayMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    BirthDay = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    NationalCode = table.Column<string>(type: "char(10)", maxLength: 10, nullable: false),
                    IdentityNumber = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false),
                    Name = table.Column<string>(maxLength: 25, nullable: false),
                    Family = table.Column<string>(maxLength: 30, nullable: false),
                    FatherName = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relative", x => x.RelativeId);
                    table.ForeignKey(
                        name: "FK_Relative_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Base",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Relative_UserId",
                schema: "Base",
                table: "Relative",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Relative",
                schema: "Base");

            migrationBuilder.DropColumn(
                name: "BirthDayMi",
                schema: "Base",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "Base",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                schema: "Base",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "BirthDay",
                schema: "Base",
                table: "User",
                type: "char(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(10)",
                oldMaxLength: 10);
        }
    }
}
