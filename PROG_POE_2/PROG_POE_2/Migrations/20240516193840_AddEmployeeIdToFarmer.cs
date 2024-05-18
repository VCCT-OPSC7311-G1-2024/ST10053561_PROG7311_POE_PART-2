using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG_POE_2.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeIdToFarmer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Farmers",
                newName: "EmployeeID");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeID",
                table: "Farmers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmployeeID",
                table: "Farmers",
                newName: "EmployeeId");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "Farmers",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
