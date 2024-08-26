using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portalBalance.Migrations
{
    /// <inheritdoc />
    public partial class addd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "PaymentFiles",
                newName: "FilePathh");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePathh",
                table: "PaymentFiles",
                newName: "FilePath");
        }
    }
}
