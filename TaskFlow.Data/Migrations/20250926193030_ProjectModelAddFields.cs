using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskFlow.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProjectModelAddFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "PM001", 0, "814a65b5-d125-410a-962f-9ef67186a233", "pm1@taskflow.com", true, "Alice ProjectManager", false, null, "PM1@TASKFLOW.COM", "PM1", "AQAAAAIAAYagAAAAELJF35JBKHC/6gY5EKcOFfYexQgIb7E6L6jNj6y/ssGKrGc+DGXwORCGWqbpclA84A==", null, false, "d439c881-0bf2-4e42-9b67-6c65019515b4", false, "pm1" },
                    { "PM002", 0, "39ed6aea-752e-42ee-a6f3-52d37548b109", "pm2@taskflow.com", true, "Bob ProjectManager", false, null, "PM2@TASKFLOW.COM", "PM2", "AQAAAAIAAYagAAAAEH95fHB4rHq5y8++SqQpRy/tVhsyN11Q8rOfYKtdGBO0kUruZlr2hbJLjBuG8TIycg==", null, false, "2602670c-c7be-4aba-8eb0-69279d0baae3", false, "pm2" },
                    { "PM003", 0, "6e30ee0b-3c1b-45b5-89cc-a62df61971dd", "pm3@taskflow.com", true, "Charlie ProjectManager", false, null, "PM3@TASKFLOW.COM", "PM3", "AQAAAAIAAYagAAAAELU/eN96gbGlrTlckZA/UMNsKncu8/eQh/8HvYdzrAp2tdLWc8Ow1UcPKLbQdjdlFg==", null, false, "4b1e491c-7189-4007-a4e4-c83c2de81640", false, "pm3" }
                });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 19, 30, 28, 910, DateTimeKind.Utc).AddTicks(2551));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 19, 30, 28, 910, DateTimeKind.Utc).AddTicks(2555));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 19, 30, 28, 910, DateTimeKind.Utc).AddTicks(2557));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "PM001");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "PM002");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "PM003");

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 19, 22, 35, 849, DateTimeKind.Utc).AddTicks(6269));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 19, 22, 35, 849, DateTimeKind.Utc).AddTicks(6271));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 26, 19, 22, 35, 849, DateTimeKind.Utc).AddTicks(6273));
        }
    }
}
