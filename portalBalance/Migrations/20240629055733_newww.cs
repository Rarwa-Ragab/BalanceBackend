using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portalBalance.Migrations
{
    /// <inheritdoc />
    public partial class newww : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePathh",
                table: "PaymentFiles");

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "PaymentFiles",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "PaymentFiles");

            migrationBuilder.AddColumn<string>(
                name: "FilePathh",
                table: "PaymentFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
