using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentApartments.Infrastructure.EntityFramework.Migrations
{
    public partial class TenantObservableApartments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApartmentTenant",
                columns: table => new
                {
                    TenantsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ObservableApartmentsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentTenant", x => new { x.TenantsId, x.ObservableApartmentsId });
                    table.ForeignKey(
                        name: "FK_ApartmentTenant_Apartments_ObservableApartmentsId",
                        column: x => x.ObservableApartmentsId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApartmentTenant_Tenants_TenantsId",
                        column: x => x.TenantsId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentTenant_ObservableApartmentsId",
                table: "ApartmentTenant",
                column: "ObservableApartmentsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApartmentTenant");
        }
    }
}
