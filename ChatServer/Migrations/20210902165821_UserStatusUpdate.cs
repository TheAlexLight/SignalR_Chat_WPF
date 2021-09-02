using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatServer.Migrations
{
    public partial class UserStatusUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            

            migrationBuilder.CreateTable(
                name: "MutesStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsMuted = table.Column<bool>(type: "bit", nullable: false),
                    UserStatusModelId = table.Column<int>(type: "int", nullable: false),
                    IsPermanent = table.Column<bool>(type: "bit", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MutesStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MutesStatus_UsersStatus_UserStatusModelId",
                        column: x => x.UserStatusModelId,
                        principalTable: "UsersStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

           

            migrationBuilder.CreateIndex(
                name: "IX_MutesStatus_UserStatusModelId",
                table: "MutesStatus",
                column: "UserStatusModelId",
                unique: true);

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MutesStatus");

        }
    }
}
