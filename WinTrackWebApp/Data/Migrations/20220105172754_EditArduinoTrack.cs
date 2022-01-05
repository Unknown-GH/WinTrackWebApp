using Microsoft.EntityFrameworkCore.Migrations;

namespace WinTrackWebApp.Data.Migrations
{
    public partial class EditArduinoTrack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DemoSwitch",
                table: "Arduino");

            migrationBuilder.AddColumn<bool>(
                name: "Track",
                table: "Arduino",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Track",
                table: "Arduino");

            migrationBuilder.AddColumn<bool>(
                name: "DemoSwitch",
                table: "Arduino",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
