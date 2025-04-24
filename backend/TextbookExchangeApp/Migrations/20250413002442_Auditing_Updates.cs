using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TextbookExchangeApp.Migrations
{
    /// <inheritdoc />
    public partial class Auditing_Updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_AspNetUsers_AuthorId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_AspNetUsers_AuthorId",
                table: "Replies");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Replies",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Replies_AuthorId",
                table: "Replies",
                newName: "IX_Replies_CreatedById");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Listings",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Listings_AuthorId",
                table: "Listings",
                newName: "IX_Listings_CreatedById");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Replies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Listings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_AspNetUsers_CreatedById",
                table: "Listings",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_AspNetUsers_CreatedById",
                table: "Replies",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_AspNetUsers_CreatedById",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_AspNetUsers_CreatedById",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Replies",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Replies_CreatedById",
                table: "Replies",
                newName: "IX_Replies_AuthorId");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Listings",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Listings_CreatedById",
                table: "Listings",
                newName: "IX_Listings_AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_AspNetUsers_AuthorId",
                table: "Listings",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_AspNetUsers_AuthorId",
                table: "Replies",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
