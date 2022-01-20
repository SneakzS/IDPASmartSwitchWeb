using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSwitchWeb.Migrations
{
    public partial class InitialCreateV5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsResent",
                table: "Message");

            migrationBuilder.CreateTable(
                name: "Workload",
                columns: table => new
                {
                    WorkloadID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    WorkloadDefinitionId = table.Column<int>(type: "INTEGER", nullable: false),
                    WorkloadW = table.Column<int>(type: "INTEGER", nullable: false),
                    DurationM = table.Column<int>(type: "INTEGER", nullable: false),
                    ToleranceDurationM = table.Column<int>(type: "INTEGER", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workload", x => x.WorkloadID);
                });

            migrationBuilder.CreateTable(
                name: "RepeatPatterns",
                columns: table => new
                {
                    RepeatPatternID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MonthFlags = table.Column<ushort>(type: "INTEGER", nullable: false),
                    DayFlags = table.Column<uint>(type: "INTEGER", nullable: false),
                    HourFlags = table.Column<uint>(type: "INTEGER", nullable: false),
                    MinuteFlags = table.Column<ulong>(type: "INTEGER", nullable: false),
                    WeekdayFlags = table.Column<ushort>(type: "INTEGER", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WorkloadID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepeatPatterns", x => x.RepeatPatternID);
                    table.ForeignKey(
                        name: "FK_RepeatPatterns_Workload_WorkloadID",
                        column: x => x.WorkloadID,
                        principalTable: "Workload",
                        principalColumn: "WorkloadID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepeatPatterns_WorkloadID",
                table: "RepeatPatterns",
                column: "WorkloadID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepeatPatterns");

            migrationBuilder.DropTable(
                name: "Workload");

            migrationBuilder.AddColumn<bool>(
                name: "IsResent",
                table: "Message",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
