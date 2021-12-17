using Microsoft.EntityFrameworkCore.Migrations;

namespace WinTrackWebApp.Data.Migrations
{
    public partial class EditArduino : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "demoData",
                table: "Arduino",
                newName: "DemoData");

            migrationBuilder.AddColumn<bool>(
                name: "DemoSwitch",
                table: "Arduino",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DemoSwitch",
                table: "Arduino");

            migrationBuilder.RenameColumn(
                name: "DemoData",
                table: "Arduino",
                newName: "demoData");
        }
    }
}
