using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jukeboxmpa.Migrations
{

    public partial class AddDurationToSong : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Songs",
                type: "INTEGER",
                nullable: true);
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Songs");
        }
    }
}
