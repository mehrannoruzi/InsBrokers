using Microsoft.EntityFrameworkCore.Migrations;

namespace InsBrokers.DataAccess.Ef.Migrations.AppDb
{
    public partial class AddUserInsuranceNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAccidentsInsurance",
                schema: "Base",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceNumber",
                schema: "Base",
                table: "User",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InsuranceNumber",
                schema: "Base",
                table: "Relative",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAccidentsInsurance",
                schema: "Base",
                table: "User");

            migrationBuilder.DropColumn(
                name: "InsuranceNumber",
                schema: "Base",
                table: "User");

            migrationBuilder.DropColumn(
                name: "InsuranceNumber",
                schema: "Base",
                table: "Relative");
        }
    }
}
