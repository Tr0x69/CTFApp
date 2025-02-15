using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CTFApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixedflag : Migration
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

            migrationBuilder.InsertData(
                table: "Flag",
                columns: new[] { "Id", "flag" },
                values: new object[] { 1, "ctf{example_flag_content}" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flag");
        }
    }
}
