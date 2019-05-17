using Microsoft.EntityFrameworkCore.Migrations;

namespace WaaS.Infrastructure.Migrations
{
    public partial class AddUrlAndFingerprint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Fingerprint",
                table: "ScrapeJobEvent",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "ScrapeJobEvent",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fingerprint",
                table: "ScrapeJobEvent");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "ScrapeJobEvent");
        }
    }
}
