using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jukeboxmpa.Migrations
{
    /// <inheritdoc />
    public partial class Credits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Credits",
                table: "Songs",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credits",
                table: "Songs");
        }
    }
}
