using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portalBalance.Migrations
{
    /// <inheritdoc />
    public partial class S_or_O_Permission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_OrgAdmins_OrgAdminId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_OrgAdmins_OrgAdminId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgAdmins_SuperAdmins_SuperAdminId",
                table: "OrgAdmins");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_OrgAdmins_OrgAdminId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Professors_OrgAdmins_OrgAdminId",
                table: "Professors");

            migrationBuilder.DropForeignKey(
                name: "FK_SubOrgs_OrgAdmins_OrgAdminId",
                table: "SubOrgs");

            migrationBuilder.DropIndex(
                name: "IX_SubOrgs_OrgAdminId",
                table: "SubOrgs");

            migrationBuilder.DropIndex(
                name: "IX_Professors_OrgAdminId",
                table: "Professors");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_OrgAdminId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Departments_OrgAdminId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Courses_OrgAdminId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "OrgAdminId",
                table: "SubOrgs");

            migrationBuilder.DropColumn(
                name: "OrgAdminId",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "OrgAdminId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "OrgAdminId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "OrgAdminId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "SuperAdminId",
                table: "OrgAdmins",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_OrgAdmins_SuperAdminId",
                table: "OrgAdmins",
                newName: "IX_OrgAdmins_OrganizationId");

            migrationBuilder.AddColumn<string>(
                name: "org_Name",
                table: "OrgAdmins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_OrgAdmins_Organizations_OrganizationId",
                table: "OrgAdmins",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgAdmins_Organizations_OrganizationId",
                table: "OrgAdmins");

            migrationBuilder.DropColumn(
                name: "org_Name",
                table: "OrgAdmins");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "OrgAdmins",
                newName: "SuperAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_OrgAdmins_OrganizationId",
                table: "OrgAdmins",
                newName: "IX_OrgAdmins_SuperAdminId");

            migrationBuilder.AddColumn<int>(
                name: "OrgAdminId",
                table: "SubOrgs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrgAdminId",
                table: "Professors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrgAdminId",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrgAdminId",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrgAdminId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubOrgs_OrgAdminId",
                table: "SubOrgs",
                column: "OrgAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Professors_OrgAdminId",
                table: "Professors",
                column: "OrgAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrgAdminId",
                table: "Organizations",
                column: "OrgAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_OrgAdminId",
                table: "Departments",
                column: "OrgAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_OrgAdminId",
                table: "Courses",
                column: "OrgAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_OrgAdmins_OrgAdminId",
                table: "Courses",
                column: "OrgAdminId",
                principalTable: "OrgAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_OrgAdmins_OrgAdminId",
                table: "Departments",
                column: "OrgAdminId",
                principalTable: "OrgAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgAdmins_SuperAdmins_SuperAdminId",
                table: "OrgAdmins",
                column: "SuperAdminId",
                principalTable: "SuperAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_OrgAdmins_OrgAdminId",
                table: "Organizations",
                column: "OrgAdminId",
                principalTable: "OrgAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Professors_OrgAdmins_OrgAdminId",
                table: "Professors",
                column: "OrgAdminId",
                principalTable: "OrgAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubOrgs_OrgAdmins_OrgAdminId",
                table: "SubOrgs",
                column: "OrgAdminId",
                principalTable: "OrgAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
