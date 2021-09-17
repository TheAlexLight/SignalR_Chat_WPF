using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatServer.Migrations
{
    public partial class UserStatusForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserModels_UsersStatus_UserStatusId",
                table: "UserModels");

            migrationBuilder.DropIndex(
                name: "IX_UserModels_UserStatusId",
                table: "UserModels");

            migrationBuilder.DropColumn(
                name: "UserStatusId",
                table: "UserModels");

            migrationBuilder.RenameColumn(
                name: "UserProfileModelId",
                table: "UsersStatus",
                newName: "UserModelId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersStatus_UserModelId",
                table: "UsersStatus",
                column: "UserModelId",
                unique: false);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersStatus_UserModels_UserModelId",
                table: "UsersStatus",
                column: "UserModelId",
                principalTable: "UserModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersStatus_UserModels_UserModelId",
                table: "UsersStatus");

            migrationBuilder.DropIndex(
                name: "IX_UsersStatus_UserModelId",
                table: "UsersStatus");

            migrationBuilder.RenameColumn(
                name: "UserModelId",
                table: "UsersStatus",
                newName: "UserProfileModelId");

            migrationBuilder.AddColumn<int>(
                name: "UserStatusId",
                table: "UserModels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserModels_UserStatusId",
                table: "UserModels",
                column: "UserStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserModels_UsersStatus_UserStatusId",
                table: "UserModels",
                column: "UserStatusId",
                principalTable: "UsersStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
