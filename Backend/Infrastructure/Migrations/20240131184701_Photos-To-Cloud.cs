using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PhotosToCloud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Photos");

            migrationBuilder.AddColumn<string>(
                name: "CloudURL",
                table: "Photos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloudURL",
                table: "Photos");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Photos",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
