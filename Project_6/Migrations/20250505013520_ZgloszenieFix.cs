using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_6.Migrations
{
    /// <inheritdoc />
    public partial class ZgloszenieFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zgloszenia_Users_UserId1",
                table: "Zgloszenia");

            migrationBuilder.DropIndex(
                name: "IX_Zgloszenia_UserId1",
                table: "Zgloszenia");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Zgloszenia");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Zgloszenia",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Zgloszenia_UserId",
                table: "Zgloszenia",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zgloszenia_Users_UserId",
                table: "Zgloszenia",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zgloszenia_Users_UserId",
                table: "Zgloszenia");

            migrationBuilder.DropIndex(
                name: "IX_Zgloszenia_UserId",
                table: "Zgloszenia");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Zgloszenia",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Zgloszenia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Zgloszenia_UserId1",
                table: "Zgloszenia",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Zgloszenia_Users_UserId1",
                table: "Zgloszenia",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
