using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatServer.Migrations
{
    public partial class ChatGroupsAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_UserModels_UserModelId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "UserModelId",
                table: "Messages",
                newName: "ChatGroupModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_UserModelId",
                table: "Messages",
                newName: "IX_Messages_ChatGroupModelId");

            migrationBuilder.AddColumn<int>(
                name: "PrivateChatId",
                table: "UserModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatGroupModelUserModel",
                columns: table => new
                {
                    GroupsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatGroupModelUserModel", x => new { x.GroupsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ChatGroupModelUserModel_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatGroupModelUserModel_UserModels_UsersId",
                        column: x => x.UsersId,
                        principalTable: "UserModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroupModelUserModel_UsersId",
                table: "ChatGroupModelUserModel",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Groups_ChatGroupModelId",
                table: "Messages",
                column: "ChatGroupModelId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Groups_ChatGroupModelId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "ChatGroupModelUserModel");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropColumn(
                name: "PrivateChatId",
                table: "UserModels");

            migrationBuilder.RenameColumn(
                name: "ChatGroupModelId",
                table: "Messages",
                newName: "UserModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ChatGroupModelId",
                table: "Messages",
                newName: "IX_Messages_UserModelId");

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_UserModels_UserModelId",
                table: "Messages",
                column: "UserModelId",
                principalTable: "UserModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
