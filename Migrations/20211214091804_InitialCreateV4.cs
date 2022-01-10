using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSwitchWeb.Migrations
{
    public partial class InitialCreateV4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "time",
                table: "Message",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "isSent",
                table: "Message",
                newName: "IsSent");

            migrationBuilder.RenameColumn(
                name: "isResent",
                table: "Message",
                newName: "IsResent");

            migrationBuilder.RenameColumn(
                name: "guid",
                table: "Message",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "message",
                table: "Message",
                newName: "MessageJson");

            migrationBuilder.AddColumn<int>(
                name: "DeviceID",
                table: "Message",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_DeviceID",
                table: "Message",
                column: "DeviceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Device_DeviceID",
                table: "Message",
                column: "DeviceID",
                principalTable: "Device",
                principalColumn: "DeviceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Device_DeviceID",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_DeviceID",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "DeviceID",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Message",
                newName: "time");

            migrationBuilder.RenameColumn(
                name: "IsSent",
                table: "Message",
                newName: "isSent");

            migrationBuilder.RenameColumn(
                name: "IsResent",
                table: "Message",
                newName: "isResent");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "Message",
                newName: "guid");

            migrationBuilder.RenameColumn(
                name: "MessageJson",
                table: "Message",
                newName: "message");
        }
    }
}
