using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDeEventos.Migrations
{
    public partial class AddEventIdToTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "event_id",
                table: "tickets",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_tickets_event_id",
                table: "tickets",
                column: "event_id");

            migrationBuilder.AddForeignKey(
                name: "fk_ticket_event",
                table: "tickets",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_ticket_event",
                table: "tickets");

            migrationBuilder.DropIndex(
                name: "ix_tickets_event_id",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "event_id",
                table: "tickets");
        }
    }
}
