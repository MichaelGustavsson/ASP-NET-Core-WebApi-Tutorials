using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace westcoastcars.api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedForeignKeyColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FuelTypeId",
                table: "Vehicles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MakeId",
                table: "Vehicles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransmissionTypeId",
                table: "Vehicles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FuelTypeId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "MakeId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TransmissionTypeId",
                table: "Vehicles");
        }
    }
}
