using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Helpdesk.Database.Migrations
{
    /// <inheritdoc />
    public partial class makeTokensUseHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRevoked",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "RefreshTokens",
                newName: "TokenHash");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "InternalNotes",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "AdminUserId",
                table: "InternalNotes",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "InternalNotes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "OnlyAdmin",
                table: "InternalNotes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_InternalNotes_TicketId",
                table: "InternalNotes",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalNotes_Tickets_TicketId",
                table: "InternalNotes",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalNotes_Tickets_TicketId",
                table: "InternalNotes");

            migrationBuilder.DropIndex(
                name: "IX_InternalNotes_TicketId",
                table: "InternalNotes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "InternalNotes");

            migrationBuilder.DropColumn(
                name: "OnlyAdmin",
                table: "InternalNotes");

            migrationBuilder.RenameColumn(
                name: "TokenHash",
                table: "RefreshTokens",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "InternalNotes",
                newName: "AdminUserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "InternalNotes",
                newName: "Note");

            migrationBuilder.AddColumn<bool>(
                name: "IsRevoked",
                table: "RefreshTokens",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
