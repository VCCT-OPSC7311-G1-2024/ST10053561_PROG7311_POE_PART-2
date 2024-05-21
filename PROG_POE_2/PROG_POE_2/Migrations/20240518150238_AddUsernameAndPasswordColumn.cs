using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG_POE_2.Migrations
{
    /// <inheritdoc />
    public partial class AddUsernameAndPasswordColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Farmers",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Farmers",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Farmers");
        }
    }
}
