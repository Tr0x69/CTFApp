using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CTFApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    flag = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userScore = table.Column<int>(type: "int", nullable: false),
                    ImageAva = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Flag",
                columns: new[] { "Id", "flag" },
                values: new object[] { 1, "CTFApp{example_flag_content}" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ImageAva", "Password", "Role", "Username", "userScore" },
                values: new object[] { "a494e680-4059-4ab6-b865-85a55eb5c165", null, "$2a$11$OcAlHbjbwP1xewGofQlSJe3226yf9Bixrp1Y.sTZtA4//BkuBp9Ge", "Admin", "admin", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flag");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
