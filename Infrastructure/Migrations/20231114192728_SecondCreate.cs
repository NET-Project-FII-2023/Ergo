using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskItemUser");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedUserUserId",
                table: "TaskItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_AssignedUserUserId",
                table: "TaskItems",
                column: "AssignedUserUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_Users_AssignedUserUserId",
                table: "TaskItems",
                column: "AssignedUserUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_Users_AssignedUserUserId",
                table: "TaskItems");

            migrationBuilder.DropIndex(
                name: "IX_TaskItems_AssignedUserUserId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "AssignedUserUserId",
                table: "TaskItems");

            migrationBuilder.CreateTable(
                name: "TaskItemUser",
                columns: table => new
                {
                    AssignedUserUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TasksTaskItemId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItemUser", x => new { x.AssignedUserUserId, x.TasksTaskItemId });
                    table.ForeignKey(
                        name: "FK_TaskItemUser_TaskItems_TasksTaskItemId",
                        column: x => x.TasksTaskItemId,
                        principalTable: "TaskItems",
                        principalColumn: "TaskItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskItemUser_Users_AssignedUserUserId",
                        column: x => x.AssignedUserUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskItemUser_TasksTaskItemId",
                table: "TaskItemUser",
                column: "TasksTaskItemId");
        }
    }
}
