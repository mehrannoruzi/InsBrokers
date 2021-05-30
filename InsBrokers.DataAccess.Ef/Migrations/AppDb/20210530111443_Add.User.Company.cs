using Microsoft.EntityFrameworkCore.Migrations;

namespace InsBrokers.DataAccess.Ef.Migrations.AppDb
{
    public partial class AddUserCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_User_UserId",
                schema: "Base",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_BankAccount_User_UserId",
                schema: "Base",
                table: "BankAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Relative_User_UserId",
                schema: "Base",
                table: "Relative");

            migrationBuilder.DropForeignKey(
                name: "FK_RelativeAttachment_Relative_RelativeId",
                schema: "Base",
                table: "RelativeAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAttachment_User_UserId",
                schema: "Base",
                table: "UserAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_Loss_User_UserId",
                schema: "Insurance",
                table: "Loss");

            migrationBuilder.DropForeignKey(
                name: "FK_LossAsset_Loss_LossId",
                schema: "Insurance",
                table: "LossAsset");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                schema: "Base",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_User_UserId",
                schema: "Base",
                table: "Address",
                column: "UserId",
                principalSchema: "Base",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Relative_User_UserId",
                schema: "Base",
                table: "Relative",
                column: "UserId",
                principalSchema: "Base",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelativeAttachment_Relative_RelativeId",
                schema: "Base",
                table: "RelativeAttachment",
                column: "RelativeId",
                principalSchema: "Base",
                principalTable: "Relative",
                principalColumn: "RelativeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAttachment_User_UserId",
                schema: "Base",
                table: "UserAttachment",
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

            migrationBuilder.AddForeignKey(
                name: "FK_LossAsset_Loss_LossId",
                schema: "Insurance",
                table: "LossAsset",
                column: "LossId",
                principalSchema: "Insurance",
                principalTable: "Loss",
                principalColumn: "LossId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_User_UserId",
                schema: "Base",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_BankAccount_User_UserId",
                schema: "Base",
                table: "BankAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Relative_User_UserId",
                schema: "Base",
                table: "Relative");

            migrationBuilder.DropForeignKey(
                name: "FK_RelativeAttachment_Relative_RelativeId",
                schema: "Base",
                table: "RelativeAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAttachment_User_UserId",
                schema: "Base",
                table: "UserAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_Loss_User_UserId",
                schema: "Insurance",
                table: "Loss");

            migrationBuilder.DropForeignKey(
                name: "FK_LossAsset_Loss_LossId",
                schema: "Insurance",
                table: "LossAsset");

            migrationBuilder.DropColumn(
                name: "Company",
                schema: "Base",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_User_UserId",
                schema: "Base",
                table: "Address",
                column: "UserId",
                principalSchema: "Base",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_Relative_User_UserId",
                schema: "Base",
                table: "Relative",
                column: "UserId",
                principalSchema: "Base",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RelativeAttachment_Relative_RelativeId",
                schema: "Base",
                table: "RelativeAttachment",
                column: "RelativeId",
                principalSchema: "Base",
                principalTable: "Relative",
                principalColumn: "RelativeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAttachment_User_UserId",
                schema: "Base",
                table: "UserAttachment",
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

            migrationBuilder.AddForeignKey(
                name: "FK_LossAsset_Loss_LossId",
                schema: "Insurance",
                table: "LossAsset",
                column: "LossId",
                principalSchema: "Insurance",
                principalTable: "Loss",
                principalColumn: "LossId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
