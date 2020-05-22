using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InsBrokers.DataAccess.Ef.Migrations
{
    public partial class initAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.CreateTable(
                name: "Action",
                schema: "Auth",
                columns: table => new
                {
                    ActionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    OrderPriority = table.Column<byte>(nullable: false),
                    ShowInMenu = table.Column<bool>(nullable: false),
                    ControllerName = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true),
                    ActionName = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true),
                    Name = table.Column<string>(maxLength: 55, nullable: false),
                    Icon = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.ActionId);
                    table.ForeignKey(
                        name: "FK_Action_Action_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Auth",
                        principalTable: "Action",
                        principalColumn: "ActionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Auth",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Enabled = table.Column<bool>(nullable: false),
                    RoleNameFa = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    RoleNameEn = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "VerificationCode",
                schema: "Auth",
                columns: table => new
                {
                    VerificationCodeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsUsed = table.Column<bool>(nullable: false),
                    UsedDateMi = table.Column<DateTime>(nullable: true),
                    InsertDateMi = table.Column<DateTime>(nullable: false),
                    InsertDateSh = table.Column<string>(type: "char(10)", maxLength: 10, nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationCode", x => x.VerificationCodeId);
                });

            migrationBuilder.CreateTable(
                name: "ActionInRole",
                schema: "Auth",
                columns: table => new
                {
                    ActionInRoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(nullable: false),
                    ActionId = table.Column<int>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionInRole", x => x.ActionInRoleId);
                    table.ForeignKey(
                        name: "FK_ActionInRole_Action_ActionId",
                        column: x => x.ActionId,
                        principalSchema: "Auth",
                        principalTable: "Action",
                        principalColumn: "ActionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionInRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInRole",
                schema: "Auth",
                columns: table => new
                {
                    UserInRoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInRole", x => x.UserInRoleId);
                    table.ForeignKey(
                        name: "FK_UserInRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Auth",
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Action_ParentId",
                schema: "Auth",
                table: "Action",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionInRole_ActionId",
                schema: "Auth",
                table: "ActionInRole",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionInRole_RoleId",
                schema: "Auth",
                table: "ActionInRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInRole_RoleId",
                schema: "Auth",
                table: "UserInRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionInRole",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "UserInRole",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "VerificationCode",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Action",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Auth");
        }
    }
}
