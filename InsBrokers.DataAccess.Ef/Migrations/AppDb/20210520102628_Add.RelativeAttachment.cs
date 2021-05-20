using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InsBrokers.DataAccess.Ef.Migrations.AppDb
{
    public partial class AddRelativeAttachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RelativeAttachment",
                schema: "Base",
                columns: table => new
                {
                    RelativeAttachmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelativeId = table.Column<int>(nullable: false),
                    FileType = table.Column<byte>(nullable: false),
                    UserAttachmentType = table.Column<int>(nullable: false),
                    Size = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    Extention = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelativeAttachment", x => x.RelativeAttachmentId);
                    table.ForeignKey(
                        name: "FK_RelativeAttachment_Relative_RelativeId",
                        column: x => x.RelativeId,
                        principalSchema: "Base",
                        principalTable: "Relative",
                        principalColumn: "RelativeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RelativeAttachment_RelativeId",
                schema: "Base",
                table: "RelativeAttachment",
                column: "RelativeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelativeAttachment",
                schema: "Base");
        }
    }
}
