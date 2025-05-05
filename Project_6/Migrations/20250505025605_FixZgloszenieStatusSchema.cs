using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_6.Migrations
{
    /// <inheritdoc />
    public partial class FixZgloszenieStatusSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZgloszenieStatusId",
                table: "Zgloszenia");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ZgloszenieStatusId",
                table: "Zgloszenia",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
