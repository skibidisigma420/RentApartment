using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentApartments.Infrastructure.EntityFramework.Migrations
{
    public partial class InitialRentApartments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Таблица Landlords
            migrationBuilder.CreateTable(
                name: "Landlords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landlords", x => x.Id);
                });

            // Таблица Tenants
            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            // Таблица Apartments
            migrationBuilder.CreateTable(
                name: "Apartments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    LandlordId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apartments_Landlords_LandlordId",
                        column: x => x.LandlordId,
                        principalTable: "Landlords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Таблица RentalAgreements
            migrationBuilder.CreateTable(
                name: "RentalAgreements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MonthlyRent_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    MonthlyRent_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    ApartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    LandlordId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalAgreements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalAgreements_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentalAgreements_Landlords_LandlordId",
                        column: x => x.LandlordId,
                        principalTable: "Landlords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentalAgreements_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Таблица RentRequests
            migrationBuilder.CreateTable(
                name: "RentRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    LandlordId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentRequests_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentRequests_Landlords_LandlordId",
                        column: x => x.LandlordId,
                        principalTable: "Landlords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentRequests_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Индексы
            migrationBuilder.CreateIndex(
                name: "IX_Apartments_LandlordId",
                table: "Apartments",
                column: "LandlordId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalAgreements_ApartmentId",
                table: "RentalAgreements",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalAgreements_LandlordId",
                table: "RentalAgreements",
                column: "LandlordId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalAgreements_TenantId",
                table: "RentalAgreements",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RentRequests_ApartmentId",
                table: "RentRequests",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RentRequests_LandlordId",
                table: "RentRequests",
                column: "LandlordId");

            migrationBuilder.CreateIndex(
                name: "IX_RentRequests_TenantId",
                table: "RentRequests",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "RentRequests");
            migrationBuilder.DropTable(name: "RentalAgreements");
            migrationBuilder.DropTable(name: "Apartments");
            migrationBuilder.DropTable(name: "Tenants");
            migrationBuilder.DropTable(name: "Landlords");
        }
    }
}
