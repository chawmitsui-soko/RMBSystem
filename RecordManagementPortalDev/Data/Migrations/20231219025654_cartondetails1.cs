using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecordManagementPortalDev.Data.Migrations
{
    public partial class cartondetails1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartonDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeptCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cartons = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CtnType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JobNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastJobNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Control = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Permanentstored = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestructDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SealNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PalletBay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    msrepl_synctran_ts = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartonDetails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartonDetails");
        }
    }
}
