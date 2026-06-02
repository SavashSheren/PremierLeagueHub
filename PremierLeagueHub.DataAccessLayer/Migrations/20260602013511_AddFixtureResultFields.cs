using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PremierLeagueHub.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddFixtureResultFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AwayScore",
                table: "Fixtures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HomeScore",
                table: "Fixtures",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayScore",
                table: "Fixtures");

            migrationBuilder.DropColumn(
                name: "HomeScore",
                table: "Fixtures");
        }
    }
}
