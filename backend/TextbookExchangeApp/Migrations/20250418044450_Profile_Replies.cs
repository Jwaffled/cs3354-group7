using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TextbookExchangeApp.Migrations
{
    /// <inheritdoc />
    public partial class Profile_Replies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Listings_ListingId",
                table: "Replies");

            migrationBuilder.AlterColumn<int>(
                name: "ListingId",
                table: "Replies",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "RecipientId",
                table: "Replies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_RecipientId",
                table: "Replies",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_AspNetUsers_RecipientId",
                table: "Replies",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Listings_ListingId",
                table: "Replies",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_AspNetUsers_RecipientId",
                table: "Replies");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Listings_ListingId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_RecipientId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Replies");

            migrationBuilder.AlterColumn<int>(
                name: "ListingId",
                table: "Replies",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Listings_ListingId",
                table: "Replies",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
