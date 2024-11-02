using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiorellaTask.Migrations
{
    public partial class SliderWordsTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "SliderWords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "SliderWords");
        }
    }
}
