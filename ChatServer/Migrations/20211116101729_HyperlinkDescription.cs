using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatServer.Migrations
{
    public partial class HyperlinkDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HyperlinkDescriptionModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageModelId = table.Column<int>(type: "int", nullable: false),
                    HyperlinkDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HyperlinkImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HyperlinkDescriptionModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HyperlinkDescriptionModel_Messages_MessageModelId",
                        column: x => x.MessageModelId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HyperlinkDescriptionModel_MessageModelId",
                table: "HyperlinkDescriptionModel",
                column: "MessageModelId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HyperlinkDescriptionModel");
        }
    }
}
