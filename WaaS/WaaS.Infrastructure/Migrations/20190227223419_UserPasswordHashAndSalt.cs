using Microsoft.EntityFrameworkCore.Migrations;

namespace WaaS.Infrastructure.Migrations
{
    public partial class UserPasswordHashAndSalt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "User",
                newName: "PasswordSalt");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "User",
                newName: "Password");
        }
    }
}
