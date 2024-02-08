using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class branchName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "TaskItems");

            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "TaskItems",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Branch",
                table: "TaskItems");

            migrationBuilder.AddColumn<string>(
                name: "BranchId",
                table: "TaskItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
