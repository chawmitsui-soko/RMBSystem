using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecordManagementPortalDev.Data.Migrations
{
    public partial class customerupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NeedApproval",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedApproval",
                table: "Customers");
        }
    }
}
