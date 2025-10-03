using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskFlow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsForJobInProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReminderJobId",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReminderSent",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "PM001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "70c03a4d-46a0-4e73-8aeb-34f8c3feafb9", "AQAAAAIAAYagAAAAEGEauUntp+tZ2x4jtph6MdRtjz6QLvBrWDVx858CQJAJdqDVJXxC8Tr0l8z/6hQINg==", "2ec04183-3aeb-42f3-94b8-538af4ef1606" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "PM002",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ca7a5a83-f56b-47f3-8395-06824185d9c3", "AQAAAAIAAYagAAAAEGBtzQmNwMUY61Wlqbe00ieSgAZdDcvLaIbJzWyLBDS83wW4HXj8sTsWbav090Oxzg==", "6717c507-1e86-4d02-b260-c61c07dfd38d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "PM003",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cdbbb95b-58f1-4342-ae52-68bf4b8c7d25", "AQAAAAIAAYagAAAAEGhWfQy9RZ+bMwJwQAoZjxaZ71R8rjMv/Lp0P053HPbvnaUjnzOJNIPtxswW2sSpZQ==", "2e3f7992-437e-410f-b3a7-63fbf235672a" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 3, 12, 48, 46, 148, DateTimeKind.Utc).AddTicks(4040));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 3, 12, 48, 46, 148, DateTimeKind.Utc).AddTicks(4043));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 3, 12, 48, 46, 148, DateTimeKind.Utc).AddTicks(4046));

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ReminderJobId", "ReminderSent" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ReminderJobId", "ReminderSent" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ReminderJobId", "ReminderSent" },
                values: new object[] { null, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReminderJobId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReminderSent",
                table: "Projects");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "PM001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "814a65b5-d125-410a-962f-9ef67186a233", "AQAAAAIAAYagAAAAELJF35JBKHC/6gY5EKcOFfYexQgIb7E6L6jNj6y/ssGKrGc+DGXwORCGWqbpclA84A==", "d439c881-0bf2-4e42-9b67-6c65019515b4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "PM002",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "39ed6aea-752e-42ee-a6f3-52d37548b109", "AQAAAAIAAYagAAAAEH95fHB4rHq5y8++SqQpRy/tVhsyN11Q8rOfYKtdGBO0kUruZlr2hbJLjBuG8TIycg==", "2602670c-c7be-4aba-8eb0-69279d0baae3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "PM003",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6e30ee0b-3c1b-45b5-89cc-a62df61971dd", "AQAAAAIAAYagAAAAELU/eN96gbGlrTlckZA/UMNsKncu8/eQh/8HvYdzrAp2tdLWc8Ow1UcPKLbQdjdlFg==", "4b1e491c-7189-4007-a4e4-c83c2de81640" });

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
    }
}
