using Microsoft.EntityFrameworkCore.Migrations;

namespace WaaS.Infrastructure.Migrations
{
    public partial class AddTypeToScrapeJobEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "HttpResponseTimeInMs",
                table: "ScrapeJobEvent",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "ScrapeJobEvent",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ScrapeJobEvent");

            migrationBuilder.AlterColumn<int>(
                name: "HttpResponseTimeInMs",
                table: "ScrapeJobEvent",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
