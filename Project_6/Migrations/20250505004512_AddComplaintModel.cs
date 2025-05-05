using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_6.Migrations
{
    /// <inheritdoc />
    public partial class AddComplaintModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZgloszenieStatuses_Zgloszenia_ZgloszenieId1",
                table: "ZgloszenieStatuses");

            migrationBuilder.DropIndex(
                name: "IX_ZgloszenieStatuses_ZgloszenieId1",
                table: "ZgloszenieStatuses");

            migrationBuilder.DropColumn(
                name: "ZgloszenieId1",
                table: "ZgloszenieStatuses");

            migrationBuilder.CreateIndex(
                name: "IX_ZgloszenieStatuses_ZgloszenieId",
                table: "ZgloszenieStatuses",
                column: "ZgloszenieId");

            migrationBuilder.AddForeignKey(
                name: "FK_ZgloszenieStatuses_Zgloszenia_ZgloszenieId",
                table: "ZgloszenieStatuses",
                column: "ZgloszenieId",
                principalTable: "Zgloszenia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZgloszenieStatuses_Zgloszenia_ZgloszenieId",
                table: "ZgloszenieStatuses");

            migrationBuilder.DropIndex(
                name: "IX_ZgloszenieStatuses_ZgloszenieId",
                table: "ZgloszenieStatuses");

            migrationBuilder.AddColumn<int>(
                name: "ZgloszenieId1",
                table: "ZgloszenieStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ZgloszenieStatuses_ZgloszenieId1",
                table: "ZgloszenieStatuses",
                column: "ZgloszenieId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ZgloszenieStatuses_Zgloszenia_ZgloszenieId1",
                table: "ZgloszenieStatuses",
                column: "ZgloszenieId1",
                principalTable: "Zgloszenia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
