using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_TaskItems_TaskItemId",
                table: "Comments");

            migrationBuilder.AlterColumn<Guid>(
                name: "TaskItemId",
                table: "Comments",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                table: "Comments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_TaskItems_TaskItemId",
                table: "Comments",
                column: "TaskItemId",
                principalTable: "TaskItems",
                principalColumn: "TaskItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_TaskItems_TaskItemId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Comments");

            migrationBuilder.AlterColumn<Guid>(
                name: "TaskItemId",
                table: "Comments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_TaskItems_TaskItemId",
                table: "Comments",
                column: "TaskItemId",
                principalTable: "TaskItems",
                principalColumn: "TaskItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
