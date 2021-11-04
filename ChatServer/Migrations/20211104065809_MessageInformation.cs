using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatServer.Migrations
{
    public partial class MessageInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Messages");

            migrationBuilder.CreateTable(
                name: "MessageInformationModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageModelId = table.Column<int>(type: "int", nullable: false),
                    TextMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageMessage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    VideoMessage = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageInformationModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageInformationModel_Messages_MessageModelId",
                        column: x => x.MessageModelId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageInformationModel_MessageModelId",
                table: "MessageInformationModel",
                column: "MessageModelId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageInformationModel");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
