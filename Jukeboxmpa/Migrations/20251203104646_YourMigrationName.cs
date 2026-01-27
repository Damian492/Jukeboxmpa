using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jukeboxmpa.Migrations
{

    public partial class YourMigrationName : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Songs",
                type: "TEXT",
                nullable: true);
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Songs");
        }
    }
}
