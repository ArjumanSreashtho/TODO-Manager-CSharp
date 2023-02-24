using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo_Manager.Migrations
{
    /// <inheritdoc />
    public partial class defaultUserAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Name", "Password", "Role", "UpdatedAt", "Username" },
                values: new object[] { new Guid("f7998e77-427f-4c55-bc67-929352dab28f"), new DateTime(2023, 2, 24, 16, 12, 27, 550, DateTimeKind.Utc).AddTicks(4688), "Arjuman Sreashtho", "$2a$12$nTQDx/njEA9wGrX1P845CenRjAf/pREoHqflQrS3EgIkExEynh3/O", "ADMIN", new DateTime(2023, 2, 24, 16, 12, 27, 550, DateTimeKind.Utc).AddTicks(4692), "Arjuman" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f7998e77-427f-4c55-bc67-929352dab28f"));

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tasks");
        }
    }
}
