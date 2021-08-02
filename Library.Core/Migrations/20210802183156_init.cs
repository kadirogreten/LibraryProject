﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Core.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LIBRARY_API_ROLES",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIBRARY_API_ROLES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LIBRARY_API_USERS",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIBRARY_API_USERS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LIBRARY_API_ROLE_CLAIMS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIBRARY_API_ROLE_CLAIMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LIBRARY_API_ROLE_CLAIMS_LIBRARY_API_ROLES_RoleId",
                        column: x => x.RoleId,
                        principalTable: "LIBRARY_API_ROLES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LIBRARY_API_USER_CLAIMS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIBRARY_API_USER_CLAIMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LIBRARY_API_USER_CLAIMS_LIBRARY_API_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "LIBRARY_API_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LIBRARY_API_USER_LOGINS",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIBRARY_API_USER_LOGINS", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_LIBRARY_API_USER_LOGINS_LIBRARY_API_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "LIBRARY_API_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LIBRARY_API_USER_ROLES",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIBRARY_API_USER_ROLES", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_LIBRARY_API_USER_ROLES_LIBRARY_API_ROLES_RoleId",
                        column: x => x.RoleId,
                        principalTable: "LIBRARY_API_ROLES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LIBRARY_API_USER_ROLES_LIBRARY_API_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "LIBRARY_API_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LIBRARY_API_USER_TOKENS",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIBRARY_API_USER_TOKENS", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_LIBRARY_API_USER_TOKENS_LIBRARY_API_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "LIBRARY_API_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LIBRARY_API_ROLE_CLAIMS_RoleId",
                table: "LIBRARY_API_ROLE_CLAIMS",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "LIBRARY_API_ROLES",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LIBRARY_API_USER_CLAIMS_UserId",
                table: "LIBRARY_API_USER_CLAIMS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LIBRARY_API_USER_LOGINS_UserId",
                table: "LIBRARY_API_USER_LOGINS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LIBRARY_API_USER_ROLES_RoleId",
                table: "LIBRARY_API_USER_ROLES",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "LIBRARY_API_USERS",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "LIBRARY_API_USERS",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LIBRARY_API_ROLE_CLAIMS");

            migrationBuilder.DropTable(
                name: "LIBRARY_API_USER_CLAIMS");

            migrationBuilder.DropTable(
                name: "LIBRARY_API_USER_LOGINS");

            migrationBuilder.DropTable(
                name: "LIBRARY_API_USER_ROLES");

            migrationBuilder.DropTable(
                name: "LIBRARY_API_USER_TOKENS");

            migrationBuilder.DropTable(
                name: "LIBRARY_API_ROLES");

            migrationBuilder.DropTable(
                name: "LIBRARY_API_USERS");
        }
    }
}
