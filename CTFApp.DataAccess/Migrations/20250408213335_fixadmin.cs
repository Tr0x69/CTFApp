using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CTFApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "a05e0a3d-5d48-4124-9c80-622812a1fb20");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ImageAva", "Password", "Role", "Username", "userScore" },
                values: new object[] { "1fe2e35d-c0b6-4dd0-84ee-139d548ee4ac", null, "$2a$11$2syZU3BzHJwopoc63ghojuE7qfB9tsZ0GVStD.V4A4hzDNZ.hd9Xy", "Admin", "admin", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1fe2e35d-c0b6-4dd0-84ee-139d548ee4ac");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ImageAva", "Password", "Role", "Username", "userScore" },
                values: new object[] { "a05e0a3d-5d48-4124-9c80-622812a1fb20", null, "$2a$11$J.gRyQD4G8fOGikXyK3pAuk771/kW4FJxTXaw1Y1qV487EfeNOiJy", "admin", "admin", 0 });
        }
    }
}
