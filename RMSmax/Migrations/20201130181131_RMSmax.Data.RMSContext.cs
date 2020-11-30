using Microsoft.EntityFrameworkCore.Migrations;

namespace RMSmax.Migrations
{
    public partial class RMSmaxDataRMSContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Content", "Title" },
                values: new object[] { 1L, "Content1", "Title1" });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Content", "Title" },
                values: new object[] { 2L, "Content2", "Title2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");
        }
    }
}
