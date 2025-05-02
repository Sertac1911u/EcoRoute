using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoRoute.Supports.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInfoToTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TicketResponses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "TicketResponses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SupportTickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "SupportTickets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TicketResponses");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TicketResponses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "SupportTickets");
        }
    }
}
