using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Core.Migrations
{
    public partial class book : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRezervation_Book_BookId",
                table: "BookRezervation");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRezervation_LIBRARY_API_USERS_UserId",
                table: "BookRezervation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookRezervation",
                table: "BookRezervation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Book",
                table: "Book");

            migrationBuilder.RenameTable(
                name: "BookRezervation",
                newName: "LIBRARY_API_BOOK_REZERVATION");

            migrationBuilder.RenameTable(
                name: "Book",
                newName: "LIBRARY_API_BOOK");

            migrationBuilder.RenameIndex(
                name: "IX_BookRezervation_UserId",
                table: "LIBRARY_API_BOOK_REZERVATION",
                newName: "IX_LIBRARY_API_BOOK_REZERVATION_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BookRezervation_BookId",
                table: "LIBRARY_API_BOOK_REZERVATION",
                newName: "IX_LIBRARY_API_BOOK_REZERVATION_BookId");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "LIBRARY_API_BOOK",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LIBRARY_API_BOOK_REZERVATION",
                table: "LIBRARY_API_BOOK_REZERVATION",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LIBRARY_API_BOOK",
                table: "LIBRARY_API_BOOK",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LIBRARY_API_BOOK_REZERVATION_LIBRARY_API_BOOK_BookId",
                table: "LIBRARY_API_BOOK_REZERVATION",
                column: "BookId",
                principalTable: "LIBRARY_API_BOOK",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LIBRARY_API_BOOK_REZERVATION_LIBRARY_API_USERS_UserId",
                table: "LIBRARY_API_BOOK_REZERVATION",
                column: "UserId",
                principalTable: "LIBRARY_API_USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LIBRARY_API_BOOK_REZERVATION_LIBRARY_API_BOOK_BookId",
                table: "LIBRARY_API_BOOK_REZERVATION");

            migrationBuilder.DropForeignKey(
                name: "FK_LIBRARY_API_BOOK_REZERVATION_LIBRARY_API_USERS_UserId",
                table: "LIBRARY_API_BOOK_REZERVATION");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LIBRARY_API_BOOK_REZERVATION",
                table: "LIBRARY_API_BOOK_REZERVATION");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LIBRARY_API_BOOK",
                table: "LIBRARY_API_BOOK");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "LIBRARY_API_BOOK");

            migrationBuilder.RenameTable(
                name: "LIBRARY_API_BOOK_REZERVATION",
                newName: "BookRezervation");

            migrationBuilder.RenameTable(
                name: "LIBRARY_API_BOOK",
                newName: "Book");

            migrationBuilder.RenameIndex(
                name: "IX_LIBRARY_API_BOOK_REZERVATION_UserId",
                table: "BookRezervation",
                newName: "IX_BookRezervation_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LIBRARY_API_BOOK_REZERVATION_BookId",
                table: "BookRezervation",
                newName: "IX_BookRezervation_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookRezervation",
                table: "BookRezervation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Book",
                table: "Book",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRezervation_Book_BookId",
                table: "BookRezervation",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRezervation_LIBRARY_API_USERS_UserId",
                table: "BookRezervation",
                column: "UserId",
                principalTable: "LIBRARY_API_USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
