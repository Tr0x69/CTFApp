using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CTFApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class adminuserAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Flag",
                keyColumn: "Id",
                keyValue: 1,
                column: "flag",
                value: "CTFApp{example_flag_content}");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ImageAva", "Password", "Role", "Username", "userScore" },
                values: new object[] { "a05e0a3d-5d48-4124-9c80-622812a1fb20", null, "$2a$11$J.gRyQD4G8fOGikXyK3pAuk771/kW4FJxTXaw1Y1qV487EfeNOiJy", "admin", "admin", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "a05e0a3d-5d48-4124-9c80-622812a1fb20");

            migrationBuilder.UpdateData(
                table: "Flag",
                keyColumn: "Id",
                keyValue: 1,
                column: "flag",
                value: "ctf{example_flag_content}");
        }
    }
}
