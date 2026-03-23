using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ong.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableCampaign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentAmount",
                table: "Campaigns",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentAmount",
                table: "Campaigns");
        }
    }
}
