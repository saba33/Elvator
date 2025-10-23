using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportLink.Infrastructure.Migrations
{
    public partial class AddFinanceModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "finance");

            migrationBuilder.CreateTable(
                name: "company_payroll_configs",
                schema: "finance",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    SalaryMode = table.Column<int>(type: "integer", nullable: false),
                    PerOrderRatePct = table.Column<decimal>(type: "numeric(5,4)", nullable: true),
                    DailyRate = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    MonthlySalary = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_payroll_configs", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_company_payroll_configs_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Method = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_payments_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payments_drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_payments_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "financial_transactions",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_financial_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_financial_transactions_payments_PaymentId",
                        column: x => x.PaymentId,
                        principalSchema: "finance",
                        principalTable: "payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_financial_transactions_CreatedAt",
                schema: "finance",
                table: "financial_transactions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_financial_transactions_PaymentId",
                schema: "finance",
                table: "financial_transactions",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_CompanyId",
                schema: "finance",
                table: "payments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_DriverId",
                schema: "finance",
                table: "payments",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_OrderId",
                schema: "finance",
                table: "payments",
                column: "OrderId");

            migrationBuilder.InsertData(
                schema: "finance",
                table: "company_payroll_configs",
                columns: new[] { "CompanyId", "SalaryMode", "PerOrderRatePct", "DailyRate", "MonthlySalary", "CreatedAt" },
                values: new object[] { new Guid("4b1a0f97-98f5-4d4d-9e9c-948d1edb5b70"), 1, 0.7000m, null, null, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "financial_transactions",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "payments",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "company_payroll_configs",
                schema: "finance");
        }
    }
}
