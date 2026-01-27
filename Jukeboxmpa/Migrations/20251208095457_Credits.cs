using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jukeboxmpa.Migrations
{

    public partial class Credits : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Credits",
                table: "Songs",
                type: "TEXT",
                nullable: true);
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credits",
                table: "Songs");
        }
    }
}
