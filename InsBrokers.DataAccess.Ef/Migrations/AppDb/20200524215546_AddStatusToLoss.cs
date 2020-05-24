using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InsBrokers.DataAccess.Ef.Migrations.AppDb
{
    public partial class AddStatusToLoss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccount_User_UserId",
                schema: "Base",
                table: "BankAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Loss_User_UserId",
                schema: "Insurance",
                table: "Loss");

            migrationBuilder.AlterColumn<byte>(
                name: "RelationType",
                schema: "Insurance",
                table: "Loss",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                schema: "Insurance",
                table: "Loss",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "LossAsset",
                schema: "Insurance",
                columns: table => new
                {
                    LossAssetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LossId = table.Column<int>(nullable: false),
                    FileType = table.Column<byte>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Extention = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: true),
                    FileUrl = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    PhysicalPath = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LossAsset", x => x.LossAssetId);
                    table.ForeignKey(
                        name: "FK_LossAsset_Loss_LossId",
                        column: x => x.LossId,
                        principalSchema: "Insurance",
                        principalTable: "Loss",
                        principalColumn: "LossId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LossAsset_LossId",
                schema: "Insurance",
                table: "LossAsset",
                column: "LossId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccount_User_UserId",
                schema: "Base",
                table: "BankAccount",
                column: "UserId",
                principalSchema: "Base",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Loss_User_UserId",
                schema: "Insurance",
                table: "Loss",
                column: "UserId",
                principalSchema: "Base",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccount_User_UserId",
                schema: "Base",
                table: "BankAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Loss_User_UserId",
                schema: "Insurance",
                table: "Loss");

            migrationBuilder.DropTable(
                name: "LossAsset",
                schema: "Insurance");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Insurance",
                table: "Loss");

            migrationBuilder.AlterColumn<string>(
                name: "RelationType",
                schema: "Insurance",
                table: "Loss",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccount_User_UserId",
                schema: "Base",
                table: "BankAccount",
                column: "UserId",
                principalSchema: "Base",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loss_User_UserId",
                schema: "Insurance",
                table: "Loss",
                column: "UserId",
                principalSchema: "Base",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
