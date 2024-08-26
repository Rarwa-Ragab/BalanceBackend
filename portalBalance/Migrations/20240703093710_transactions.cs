using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portalBalance.Migrations
{
    /// <inheritdoc />
    public partial class transactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NationalID",
                table: "Professors",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "CourseBalance",
                table: "Courses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Professors_NationalID",
                table: "Professors",
                column: "NationalID");

            migrationBuilder.CreateTable(
                name: "CourseTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionCount = table.Column<int>(type: "int", nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceBefore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UniversityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacultyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcademicYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTransactions_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfessorTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NationalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionReference = table.Column<int>(type: "int", nullable: false),
                    Reference2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvailableBalanceBefore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailableBalanceAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UniversityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacultyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessorTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfessorTransaction_Professors_NationalId",
                        column: x => x.NationalId,
                        principalTable: "Professors",
                        principalColumn: "NationalID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Professors_NationalID",
                table: "Professors",
                column: "NationalID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseTransactions_CourseId",
                table: "CourseTransactions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorTransaction_NationalId",
                table: "ProfessorTransaction",
                column: "NationalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseTransactions");

            migrationBuilder.DropTable(
                name: "ProfessorTransaction");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Professors_NationalID",
                table: "Professors");

            migrationBuilder.DropIndex(
                name: "IX_Professors_NationalID",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "CourseBalance",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "Courses");

            migrationBuilder.AlterColumn<string>(
                name: "NationalID",
                table: "Professors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
