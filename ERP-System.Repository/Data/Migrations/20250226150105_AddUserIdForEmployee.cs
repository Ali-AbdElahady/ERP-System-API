using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_System.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdForEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Employees");
        }
    }
}
