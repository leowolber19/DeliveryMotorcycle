using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryMotorcycle.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StatusMotorcycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Motorcycle",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Motorcycle");
        }
    }
}
