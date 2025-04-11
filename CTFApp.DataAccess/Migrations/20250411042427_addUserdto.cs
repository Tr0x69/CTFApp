using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CTFApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addUserdto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1fe2e35d-c0b6-4dd0-84ee-139d548ee4ac");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ImageAva", "Password", "Role", "Username", "userScore" },
                values: new object[] { "c3648669-7a6f-42d1-88e6-44f6b6a9b57e", null, "$2a$11$.TU.36GokhZBVmZRro9Cpeq0ubJPS6sLV5kWkw.dC1R0/RsNWynIe", "Admin", "admin", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "c3648669-7a6f-42d1-88e6-44f6b6a9b57e");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ImageAva", "Password", "Role", "Username", "userScore" },
                values: new object[] { "1fe2e35d-c0b6-4dd0-84ee-139d548ee4ac", null, "$2a$11$2syZU3BzHJwopoc63ghojuE7qfB9tsZ0GVStD.V4A4hzDNZ.hd9Xy", "Admin", "admin", 0 });
        }
    }
}
