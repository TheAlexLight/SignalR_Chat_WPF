using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatServer.Migrations
{
    public partial class UserProfileForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_UserProfileModel_UserInfoId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersStatus_AspNetUsers_UserId",
                table: "UsersStatus");

            migrationBuilder.DropIndex(
                name: "IX_UsersStatus_UserId",
                table: "UsersStatus");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserInfoId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfileModel",
                table: "UserProfileModel");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UsersStatus");

            migrationBuilder.DropColumn(
                name: "UserInfoId",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "UserProfileModel",
                newName: "UserProfiles");

            migrationBuilder.AddColumn<int>(
                name: "UserProfileModelId",
                table: "UsersStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserModelId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserModelId",
                table: "UserProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserStatusId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserModels_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserModels_UsersStatus_UserStatusId",
                        column: x => x.UserStatusId,
                        principalTable: "UsersStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserModelId",
                table: "Messages",
                column: "UserModelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserModelId",
                table: "UserProfiles",
                column: "UserModelId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserModels_UserId",
                table: "UserModels",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserModels_UserStatusId",
                table: "UserModels",
                column: "UserStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_UserModels_UserModelId",
                table: "Messages",
                column: "UserModelId",
                principalTable: "UserModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_UserModels_UserModelId",
                table: "UserProfiles",
                column: "UserModelId",
                principalTable: "UserModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_UserModels_UserModelId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_UserModels_UserModelId",
                table: "UserProfiles");

            migrationBuilder.DropTable(
                name: "UserModels");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UserModelId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_UserModelId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "UserProfileModelId",
                table: "UsersStatus");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "UserProfiles");

            migrationBuilder.RenameTable(
                name: "UserProfiles",
                newName: "UserProfileModel");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UsersStatus",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserInfoId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfileModel",
                table: "UserProfileModel",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UsersStatus_UserId",
                table: "UsersStatus",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserInfoId",
                table: "Messages",
                column: "UserInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_UserProfileModel_UserInfoId",
                table: "Messages",
                column: "UserInfoId",
                principalTable: "UserProfileModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersStatus_AspNetUsers_UserId",
                table: "UsersStatus",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
