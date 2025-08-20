using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcMovie.Migrations
{
    /// <inheritdoc />
    public partial class AddInscriptionRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FormationId = table.Column<int>(type: "int", nullable: false),
                    EmployeId = table.Column<int>(type: "int", nullable: false),
                    DateInscription = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    Presence = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CertificatEmis = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CertificatNumero = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateDelivrance = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inscriptions_Employes_EmployeId",
                        column: x => x.EmployeId,
                        principalTable: "Employes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscriptions_Formations_FormationId",
                        column: x => x.FormationId,
                        principalTable: "Formations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_EmployeId",
                table: "Inscriptions",
                column: "EmployeId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_FormationId_EmployeId",
                table: "Inscriptions",
                columns: new[] { "FormationId", "EmployeId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inscriptions");
        }
    }
}
