using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XpressAccess.Identity.Service.Data.Migrations.IdentityDb
{
    public partial class InitialIdentityMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "XARole",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XARole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "XAIdentity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccessAllowed = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XAIdentity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "XAUserRole",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XAUserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_XAUserRole_XARole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "XARole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_XAUserRole_XAIdentity_UserId",
                        column: x => x.UserId,
                        principalTable: "XAIdentity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "XAIdentity",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_XAUserRole_RoleId",
                table: "XAUserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "XAUserRole");

            migrationBuilder.DropTable(
                name: "XARole");

            migrationBuilder.DropTable(
                name: "XAIdentity");
        }
    }
}
