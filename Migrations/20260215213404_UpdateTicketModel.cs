using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDeEventos.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTicketModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_rating_order",
                table: "ratings");

            migrationBuilder.DropPrimaryKey(
                name: "ratings_pkey",
                table: "ratings");

            migrationBuilder.DropColumn(
                name: "note",
                table: "ratings");

            migrationBuilder.RenameColumn(
                name: "order_id",
                table: "ratings",
                newName: "OrderId");

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "accessibility",
                table: "tickets",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "event_id",
                table: "tickets",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "ratings",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "ratings",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "ratings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<Guid>(
                name: "event_id",
                table: "ratings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "score",
                table: "ratings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "ratings_pkey",
                table: "ratings",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_event_id",
                table: "tickets",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_ratings_OrderId",
                table: "ratings",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ratings_orders_OrderId",
                table: "ratings",
                column: "OrderId",
                principalTable: "orders",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_ticket_event",
                table: "tickets",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ratings_orders_OrderId",
                table: "ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_events_EventId1",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "fk_ticket_event",
                table: "tickets");

            migrationBuilder.DropIndex(
                name: "IX_tickets_event_id",
                table: "tickets");

            migrationBuilder.DropPrimaryKey(
                name: "ratings_pkey",
                table: "ratings");

            migrationBuilder.DropIndex(
                name: "IX_ratings_OrderId",
                table: "ratings");

            migrationBuilder.DropColumn(
                name: "password",
                table: "users");

            migrationBuilder.DropColumn(
                name: "event_id",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "id",
                table: "ratings");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "ratings");

            migrationBuilder.DropColumn(
                name: "event_id",
                table: "ratings");

            migrationBuilder.DropColumn(
                name: "score",
                table: "ratings");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "ratings",
                newName: "order_id");

            migrationBuilder.AlterColumn<string>(
                name: "accessibility",
                table: "tickets",
                type: "character varying(100)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "order_id",
                table: "ratings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "note",
                table: "ratings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "ratings_pkey",
                table: "ratings",
                columns: new[] { "order_id", "user_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_rating_order",
                table: "ratings",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
