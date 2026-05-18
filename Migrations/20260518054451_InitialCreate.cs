using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIN_INS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InsuranceCompanies",
                columns: table => new
                {
                    CompanyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DefaultCoverageRatio = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceCompanies", x => x.CompanyID);
                });

            migrationBuilder.CreateTable(
                name: "MedicalServices",
                columns: table => new
                {
                    ServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ServicePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalServices", x => x.ServiceID);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    NationalID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DigitalID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BloodType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Allergies = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.NationalID);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    InsuranceCoveredAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PatientPayableAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceID);
                    table.ForeignKey(
                        name: "FK_Invoices_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "NationalID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientInsurances",
                columns: table => new
                {
                    InsuranceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    CoverageLimit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrentBalanceUsed = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientInsurances", x => x.InsuranceID);
                    table.ForeignKey(
                        name: "FK_PatientInsurances_InsuranceCompanies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "InsuranceCompanies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientInsurances_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "NationalID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    ItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceID = table.Column<int>(type: "int", nullable: false),
                    ServiceID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.ItemID);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoices_InvoiceID",
                        column: x => x.InvoiceID,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_MedicalServices_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "MedicalServices",
                        principalColumn: "ServiceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceID",
                table: "InvoiceItems",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_ServiceID",
                table: "InvoiceItems",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PatientID",
                table: "Invoices",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientInsurances_CompanyID",
                table: "PatientInsurances",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientInsurances_PatientID",
                table: "PatientInsurances",
                column: "PatientID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "PatientInsurances");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "MedicalServices");

            migrationBuilder.DropTable(
                name: "InsuranceCompanies");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
