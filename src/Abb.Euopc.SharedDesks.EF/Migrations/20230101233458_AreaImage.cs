using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Abb.Euopc.SharedDesks.EF.Migrations
{
    public partial class AreaImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Areas",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Areas");
        }
    }
}
