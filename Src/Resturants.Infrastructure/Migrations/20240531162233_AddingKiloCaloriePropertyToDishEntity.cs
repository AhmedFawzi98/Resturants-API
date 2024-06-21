using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resturants.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingKiloCaloriePropertyToDishEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KiloCalorie",
                table: "Dishes",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KiloCalorie",
                table: "Dishes");
        }
    }
}
