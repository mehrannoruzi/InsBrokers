using Microsoft.EntityFrameworkCore.Migrations;

namespace InsBrokers.DataAccess.Ef.Migrations.AppDb
{
    public partial class EditUserAttachmentProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                schema: "Base",
                table: "UserAttachment",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<long>(
                name: "Size",
                schema: "Base",
                table: "UserAttachment",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Base",
                table: "UserAttachment",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16)",
                oldMaxLength: 16);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                schema: "Base",
                table: "UserAttachment",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<int>(
                name: "Size",
                schema: "Base",
                table: "UserAttachment",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Base",
                table: "UserAttachment",
                type: "varchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);
        }
    }
}
