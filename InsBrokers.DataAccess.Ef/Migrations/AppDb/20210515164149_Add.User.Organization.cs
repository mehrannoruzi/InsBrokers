using Microsoft.EntityFrameworkCore.Migrations;

namespace InsBrokers.DataAccess.Ef.Migrations.AppDb
{
    public partial class AddUserOrganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InsurancePlan",
                schema: "Base",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Organization",
                schema: "Base",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsurancePlan",
                schema: "Base",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Organization",
                schema: "Base",
                table: "User");
        }
    }
}
