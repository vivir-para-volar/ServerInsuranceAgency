using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class DBInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationPlate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehiclePassport = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Passport = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PersonAllowedToDrives",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrivingLicence = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonAllowedToDrives", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Policyholders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Passport = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policyholders", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Policies",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsuranceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsurancePremium = table.Column<int>(type: "int", nullable: false),
                    InsuranceAmount = table.Column<int>(type: "int", nullable: false),
                    DateOfConclusion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PolicyholderID = table.Column<int>(type: "int", nullable: false),
                    CarID = table.Column<int>(type: "int", nullable: false),
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policies", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Policies_Cars_CarID",
                        column: x => x.CarID,
                        principalTable: "Cars",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Policies_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Policies_Policyholders_PolicyholderID",
                        column: x => x.PolicyholderID,
                        principalTable: "Policyholders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceEvents",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsurancePayment = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PolicyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceEvents", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InsuranceEvents_Policies_PolicyID",
                        column: x => x.PolicyID,
                        principalTable: "Policies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonAllowedToDrivePolicy",
                columns: table => new
                {
                    PersonsAllowedToDriveID = table.Column<int>(type: "int", nullable: false),
                    PoliciesID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonAllowedToDrivePolicy", x => new { x.PersonsAllowedToDriveID, x.PoliciesID });
                    table.ForeignKey(
                        name: "FK_PersonAllowedToDrivePolicy_PersonAllowedToDrives_PersonsAllowedToDriveID",
                        column: x => x.PersonsAllowedToDriveID,
                        principalTable: "PersonAllowedToDrives",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonAllowedToDrivePolicy_Policies_PoliciesID",
                        column: x => x.PoliciesID,
                        principalTable: "Policies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceEvents_PolicyID",
                table: "InsuranceEvents",
                column: "PolicyID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonAllowedToDrivePolicy_PoliciesID",
                table: "PersonAllowedToDrivePolicy",
                column: "PoliciesID");

            migrationBuilder.CreateIndex(
                name: "IX_Policies_CarID",
                table: "Policies",
                column: "CarID");

            migrationBuilder.CreateIndex(
                name: "IX_Policies_EmployeeID",
                table: "Policies",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Policies_PolicyholderID",
                table: "Policies",
                column: "PolicyholderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsuranceEvents");

            migrationBuilder.DropTable(
                name: "PersonAllowedToDrivePolicy");

            migrationBuilder.DropTable(
                name: "PersonAllowedToDrives");

            migrationBuilder.DropTable(
                name: "Policies");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Policyholders");
        }
    }
}
