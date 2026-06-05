using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Helpdesk.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameCreatedByUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreadtedByUserId",
                table: "Tickets",
                newName: "CreatedByUserId");

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Tickets",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CreatedByUserId",
                table: "Tickets",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalNotes_UserId",
                table: "InternalNotes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalNotes_Users_UserId",
                table: "InternalNotes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_CreatedByUserId",
                table: "Tickets",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalNotes_Users_UserId",
                table: "InternalNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_CreatedByUserId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_CreatedByUserId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_InternalNotes_UserId",
                table: "InternalNotes");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Tickets",
                newName: "CreadtedByUserId");
        }
    }
}
