using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatServer.Migrations
{
    public partial class ProfileImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "UserProfiles");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "UserProfiles",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "UserProfiles");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
