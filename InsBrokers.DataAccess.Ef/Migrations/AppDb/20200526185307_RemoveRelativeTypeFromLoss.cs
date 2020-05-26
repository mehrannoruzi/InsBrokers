using Microsoft.EntityFrameworkCore.Migrations;

namespace InsBrokers.DataAccess.Ef.Migrations.AppDb
{
    public partial class RemoveRelativeTypeFromLoss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelationType",
                schema: "Insurance",
                table: "Loss");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "RelationType",
                schema: "Insurance",
                table: "Loss",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
