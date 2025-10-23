using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportLink.Infrastructure.Migrations
{
    public partial class AddCompaniesAndDrivers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ContactEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "drivers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Rating = table.Column<decimal>(type: "numeric(3,2)", nullable: false, defaultValue: 0m),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    VehicleType = table.Column<int>(type: "integer", nullable: false),
                    ActiveAssignments = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_drivers_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_drivers_CompanyId",
                table: "drivers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_DriverId",
                table: "orders",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_drivers_DriverId",
                table: "orders",
                column: "DriverId",
                principalTable: "drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.InsertData(
                table: "companies",
                columns: new[] { "Id", "Name", "Address", "ContactEmail" },
                values: new object[,]
                {
                    { new Guid("4b1a0f97-98f5-4d4d-9e9c-948d1edb5b70"), "Logistics Hub", "100 Market Street, Springfield", "contact@logisticshub.com" },
                    { new Guid("c31a68a0-8ff1-4020-8bd2-1651a8651adb"), "Express Freight", "200 Cargo Road, Metropolis", "ops@expressfreight.io" }
                });

            migrationBuilder.InsertData(
                table: "drivers",
                columns: new[] { "Id", "CompanyId", "Name", "Phone", "Rating", "IsAvailable", "VehicleType", "ActiveAssignments" },
                values: new object[,]
                {
                    { new Guid("3c7d5a4f-54b4-4f48-8b1e-7058ff5a9c11"), new Guid("4b1a0f97-98f5-4d4d-9e9c-948d1edb5b70"), "Alice Walker", "+1-555-0101", 4.8m, true, 1, 0 },
                    { new Guid("5ab7136f-0c1a-4b60-8d13-59c9c7bd08bf"), new Guid("4b1a0f97-98f5-4d4d-9e9c-948d1edb5b70"), "Bob Nguyen", "+1-555-0102", 4.5m, true, 0, 0 },
                    { new Guid("f47b4b82-7fb3-4c36-9e39-2adce37b329e"), new Guid("c31a68a0-8ff1-4020-8bd2-1651a8651adb"), "Carlos Diaz", "+1-555-0103", 4.9m, true, 2, 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_drivers_DriverId",
                table: "orders");

            migrationBuilder.DropTable(
                name: "drivers");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropIndex(
                name: "IX_orders_DriverId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "orders");
        }
    }
}
