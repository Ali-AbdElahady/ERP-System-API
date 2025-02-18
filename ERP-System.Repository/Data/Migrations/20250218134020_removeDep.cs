using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_System.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class removeDep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
