using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecordManagementPortalDev.Data.Migrations
{
    public partial class passwordexp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPasswordChangedDate",
                table: "AppUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "CtnMonthlyClosingBal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartonType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClosingBalance = table.Column<int>(type: "int", nullable: false),
                    AsatDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CtnMonthlyClosingBal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CtnStockReceiving",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartonType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceivingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CtnStockReceiving", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CtnMonthlyClosingBal");

            migrationBuilder.DropTable(
                name: "CtnStockReceiving");

            migrationBuilder.DropColumn(
                name: "LastPasswordChangedDate",
                table: "AppUsers");

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cartons = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chopandsign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Control = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CtnType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeptCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestructDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JobNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastJobNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldCust = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PalletBay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Permanentstored = table.Column<bool>(type: "bit", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SealNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubFiles = table.Column<int>(type: "int", nullable: true),
                    TransDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });
        }
    }
}
