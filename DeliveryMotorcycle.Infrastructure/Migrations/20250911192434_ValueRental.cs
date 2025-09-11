using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryMotorcycle.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ValueRental : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "Rentals",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Rentals");
        }
    }
}
