using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalMarketplaceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddBodyTypeAndRentalLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PickupLocation",
                table: "Rentals",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReturnLocation",
                table: "Rentals",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BodyType",
                table: "Cars",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickupLocation",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "ReturnLocation",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "BodyType",
                table: "Cars");
        }
    }
}
