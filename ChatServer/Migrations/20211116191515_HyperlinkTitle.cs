using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatServer.Migrations
{
    public partial class HyperlinkTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HyperlinkTitle",
                table: "HyperlinkDescriptionModel",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HyperlinkTitle",
                table: "HyperlinkDescriptionModel");
        }
    }
}
