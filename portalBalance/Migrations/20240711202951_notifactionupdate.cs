using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portalBalance.Migrations
{
    /// <inheritdoc />
    public partial class notifactionupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableBalanceAfter",
                table: "NotificationLogs");

            migrationBuilder.DropColumn(
                name: "NotificationDate",
                table: "NotificationLogs");

            migrationBuilder.DropColumn(
                name: "NotificationSent",
                table: "NotificationLogs");

            migrationBuilder.DropColumn(
                name: "ProfessorName",
                table: "NotificationLogs");

            migrationBuilder.DropColumn(
                name: "TransactionAmount",
                table: "NotificationLogs");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "NotificationLogs");

            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "NotificationLogs",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "NotificationLogs",
                newName: "Body");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "NotificationLogs",
                newName: "TransactionType");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "NotificationLogs",
                newName: "Status");

            migrationBuilder.AddColumn<decimal>(
                name: "AvailableBalanceAfter",
                table: "NotificationLogs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "NotificationDate",
                table: "NotificationLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "NotificationSent",
                table: "NotificationLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorName",
                table: "NotificationLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TransactionAmount",
                table: "NotificationLogs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "NotificationLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
