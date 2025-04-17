using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TextbookExchangeApp.Migrations
{
    /// <inheritdoc />
    public partial class Add_Ratings_To_Replies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Replies",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Replies");
        }
    }
}
