using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backoffice_ANCFCC.Migrations
{
    /// <inheritdoc />
    public partial class MigrationReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrateur",
                columns: table => new
                {
                    AdministrateurId = table.Column<int>(type: "int", nullable: false),
                    Nom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Prenom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    MotDePasse = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Administ__5AAEDD3CEBBE43CC", x => x.AdministrateurId);
                });

            migrationBuilder.CreateTable(
                name: "Agent",
                columns: table => new
                {
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    Nom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Prenom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    MotDePasse = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Agent__9AC3BFF1C2C7BF3A", x => x.AgentId);
                });

            migrationBuilder.CreateTable(
                name: "Candidats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adresse = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    AdresseArabe = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    AvoirQualiteResistant = table.Column<bool>(type: "bit", nullable: true),
                    Cin = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    CodePostal = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    DateNaissance = table.Column<DateTime>(type: "date", nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    EstHandicape = table.Column<bool>(type: "bit", nullable: true),
                    Genre = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Matrimoniale = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Nom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    NomArabe = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Prenom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PrenomArabe = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Telephone1 = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Telephone2 = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Ville = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    EstFonctionnaire = table.Column<bool>(type: "bit", nullable: true),
                    AvoirQualiteMilitaire = table.Column<bool>(type: "bit", nullable: true),
                    AvoirQualitePupille = table.Column<bool>(type: "bit", nullable: true),
                    AvoirQualiteComBattant = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Candidat__3214EC07F3250E92", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Concours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateExpiration = table.Column<DateTime>(type: "date", nullable: true),
                    DatePublication = table.Column<DateTime>(type: "date", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Nom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Diplome = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Domaine = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Concours__3214EC076D49CFE5", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Candidatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidatsId = table.Column<int>(type: "int", nullable: true),
                    ConcoursId = table.Column<int>(type: "int", nullable: true),
                    DatePostulation = table.Column<DateTime>(type: "date", nullable: true),
                    Reference = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    StatutDoc = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CommentaireDoc = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    StatutExp = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CommentaireExp = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    StatutInfoPerso = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CommentaireInfoPerso = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    AdministrateurId = table.Column<int>(type: "int", nullable: true),
                    CommentaireFormation = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    StatutFormation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    DateModification = table.Column<DateTime>(type: "datetime", nullable: true),
                    recapUser = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    statusGlobal = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    StatusAgent = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    AgentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Candidat__3214EC0725A922E0", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidatures_Administrateur",
                        column: x => x.AdministrateurId,
                        principalTable: "Administrateur",
                        principalColumn: "AdministrateurId");
                    table.ForeignKey(
                        name: "FK_Candidatures_Agent",
                        column: x => x.AgentId,
                        principalTable: "Agent",
                        principalColumn: "AgentId");
                    table.ForeignKey(
                        name: "FK__Candidatu__Candi__5629CD9C",
                        column: x => x.CandidatsId,
                        principalTable: "Candidats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Candidatu__Conco__571DF1D5",
                        column: x => x.ConcoursId,
                        principalTable: "Concours",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ConcoursAttachements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Chemin = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ConcoursId = table.Column<int>(type: "int", nullable: true),
                    Nom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Concours__3214EC07AC9DE213", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ConcoursA__Conco__628FA481",
                        column: x => x.ConcoursId,
                        principalTable: "Concours",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Diplomes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Anne = table.Column<int>(type: "int", nullable: true),
                    AuMaroc = table.Column<bool>(type: "bit", nullable: true),
                    AutreDiplome = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CandidatureId = table.Column<int>(type: "int", nullable: true),
                    EstSimilaire = table.Column<bool>(type: "bit", nullable: true),
                    Etablissement = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Nom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Note = table.Column<double>(type: "float", nullable: true),
                    OptionDiplome = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Diplomes__3214EC071E66C4A7", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Diplomes__Candid__59FA5E80",
                        column: x => x.CandidatureId,
                        principalTable: "Candidatures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidatureId = table.Column<int>(type: "int", nullable: true),
                    Chemin = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Nom = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Statut = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Commentaire = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Document__3214EC0733786CD9", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Documents__Candi__5CD6CB2B",
                        column: x => x.CandidatureId,
                        principalTable: "Candidatures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Experiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidatureId = table.Column<int>(type: "int", nullable: true),
                    DomaineActivite = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    NombreAnnee = table.Column<int>(type: "int", nullable: true),
                    Poste = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Experien__3214EC07E68FF620", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Experienc__Candi__5FB337D6",
                        column: x => x.CandidatureId,
                        principalTable: "Candidatures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidatures_AdministrateurId",
                table: "Candidatures",
                column: "AdministrateurId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidatures_AgentId",
                table: "Candidatures",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidatures_CandidatsId",
                table: "Candidatures",
                column: "CandidatsId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidatures_ConcoursId",
                table: "Candidatures",
                column: "ConcoursId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcoursAttachements_ConcoursId",
                table: "ConcoursAttachements",
                column: "ConcoursId");

            migrationBuilder.CreateIndex(
                name: "IX_Diplomes_CandidatureId",
                table: "Diplomes",
                column: "CandidatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CandidatureId",
                table: "Documents",
                column: "CandidatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_CandidatureId",
                table: "Experiences",
                column: "CandidatureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConcoursAttachements");

            migrationBuilder.DropTable(
                name: "Diplomes");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Experiences");

            migrationBuilder.DropTable(
                name: "Candidatures");

            migrationBuilder.DropTable(
                name: "Administrateur");

            migrationBuilder.DropTable(
                name: "Agent");

            migrationBuilder.DropTable(
                name: "Candidats");

            migrationBuilder.DropTable(
                name: "Concours");
        }
    }
}
