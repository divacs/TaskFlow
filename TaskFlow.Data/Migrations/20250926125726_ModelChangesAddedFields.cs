using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskFlow.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModelChangesAddedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedUserId",
                table: "TaskItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectManagerId",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 12, 57, 25, 617, DateTimeKind.Utc).AddTicks(3293));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 12, 57, 25, 617, DateTimeKind.Utc).AddTicks(3296));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 12, 57, 25, 617, DateTimeKind.Utc).AddTicks(3299));

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProjectManagerId",
                value: "PM001");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProjectManagerId",
                value: "PM002");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProjectManagerId",
                value: "PM003");

            migrationBuilder.UpdateData(
                table: "TaskItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "AssignedUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TaskItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "AssignedUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TaskItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "AssignedUserId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_AssignedUserId",
                table: "TaskItems",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_AspNetUsers_AssignedUserId",
                table: "TaskItems",
                column: "AssignedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_ProjectManagerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_AspNetUsers_AssignedUserId",
                table: "TaskItems");

            migrationBuilder.DropIndex(
                name: "IX_TaskItems_AssignedUserId",
                table: "TaskItems");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjectManagerId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "ProjectManagerId",
                table: "Projects");

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 25, 13, 27, 38, 777, DateTimeKind.Utc).AddTicks(5762));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 25, 13, 27, 38, 777, DateTimeKind.Utc).AddTicks(5765));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 25, 13, 27, 38, 777, DateTimeKind.Utc).AddTicks(5767));
        }
    }
}
