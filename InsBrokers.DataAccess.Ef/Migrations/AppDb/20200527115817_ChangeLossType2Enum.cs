using Microsoft.EntityFrameworkCore.Migrations;

namespace InsBrokers.DataAccess.Ef.Migrations.AppDb
{
    public partial class ChangeLossType2Enum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "LossType",
                schema: "Insurance",
                table: "Loss",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LossType",
                schema: "Insurance",
                table: "Loss",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(byte));
        }
    }
}
