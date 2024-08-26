using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portalBalance.Migrations
{
    /// <inheritdoc />
    public partial class uploadedcsv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EducationYear",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "UniversityExpenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillTypeCode = table.Column<int>(type: "int", nullable: false),
                    BillReferenceNo = table.Column<long>(type: "bigint", nullable: false),
                    BillTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalId = table.Column<long>(type: "bigint", nullable: false),
                    StudentCode = table.Column<long>(type: "bigint", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TuitionFees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookFees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReconStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacultyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacultyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassSemester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudyNature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AsNode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhaseNode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCalculated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniversityExpenses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UniversityExpenses");

            migrationBuilder.AlterColumn<int>(
                name: "EducationYear",
                table: "Courses",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
