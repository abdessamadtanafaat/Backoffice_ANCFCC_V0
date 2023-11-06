﻿// <auto-generated />
using System;
using Backoffice_ANCFCC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backoffice_ANCFCC.Migrations
{
    [DbContext(typeof(DbAncfccContext))]
    [Migration("20230820221712_USINGDB1")]
    partial class USINGDB1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Administrateur", b =>
                {
                    b.Property<int>("AdministrateurId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("MotDePasse")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AdministrateurId")
                        .HasName("PK__Administ__5AAEDD3CEBBE43CC");

                    b.ToTable("Administrateur", (string)null);
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Agent", b =>
                {
                    b.Property<int>("AgentId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("MotDePasse")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AgentId")
                        .HasName("PK__Agent__9AC3BFF1C2C7BF3A");

                    b.ToTable("Agent", (string)null);
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Candidat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adresse")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("AdresseArabe")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<bool?>("AvoirQualiteComBattant")
                        .HasColumnType("bit");

                    b.Property<bool?>("AvoirQualiteMilitaire")
                        .HasColumnType("bit");

                    b.Property<bool?>("AvoirQualitePupille")
                        .HasColumnType("bit");

                    b.Property<bool?>("AvoirQualiteResistant")
                        .HasColumnType("bit");

                    b.Property<string>("Cin")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodePostal")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<DateTime?>("DateNaissance")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<bool?>("EstFonctionnaire")
                        .HasColumnType("bit");

                    b.Property<bool?>("EstHandicape")
                        .HasColumnType("bit");

                    b.Property<string>("Genre")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Matrimoniale")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Nom")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NomArabe")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Prenom")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PrenomArabe")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Telephone1")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Telephone2")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Ville")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id")
                        .HasName("PK__Candidat__3214EC07F3250E92");

                    b.ToTable("Candidats");
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Candidature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AdministrateurId")
                        .HasColumnType("int");

                    b.Property<int?>("AgentId")
                        .HasColumnType("int");

                    b.Property<int?>("CandidatsId")
                        .HasColumnType("int");

                    b.Property<string>("CommentaireDoc")
                        .HasMaxLength(1000)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("CommentaireExp")
                        .HasMaxLength(1000)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("CommentaireFormation")
                        .HasMaxLength(1000)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("CommentaireInfoPerso")
                        .HasMaxLength(1000)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1000)");

                    b.Property<int?>("ConcoursId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateModification")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DatePostulation")
                        .HasColumnType("date");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("StatusAgent")
                        .HasColumnType("int");

                    b.Property<int?>("StatusGlobal")
                        .HasColumnType("int")
                        .HasColumnName("statusGlobal");

                    b.Property<int?>("StatutDoc")
                        .HasColumnType("int");

                    b.Property<int?>("StatutExp")
                        .HasColumnType("int");

                    b.Property<int?>("StatutFormation")
                        .HasColumnType("int");

                    b.Property<int?>("StatutInfoPerso")
                        .HasColumnType("int");

                    b.Property<bool>("Verrouille")
                        .HasColumnType("bit");

                    b.HasKey("Id")
                        .HasName("PK__Candidat__3214EC0725A922E0");

                    b.HasIndex("AdministrateurId");

                    b.HasIndex("AgentId");

                    b.HasIndex("CandidatsId");

                    b.HasIndex("ConcoursId");

                    b.ToTable("Candidatures");
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Concour", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DateExpiration")
                        .HasColumnType("date");

                    b.Property<DateTime?>("DatePublication")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Diplome")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Domaine")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Nom")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id")
                        .HasName("PK__Concours__3214EC076D49CFE5");

                    b.ToTable("Concours");
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.ConcoursAttachement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Chemin")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("ConcoursId")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id")
                        .HasName("PK__Concours__3214EC07AC9DE213");

                    b.HasIndex("ConcoursId");

                    b.ToTable("ConcoursAttachements");
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Diplome", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Anne")
                        .HasColumnType("int");

                    b.Property<bool?>("AuMaroc")
                        .HasColumnType("bit");

                    b.Property<string>("AutreDiplome")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("CandidatureId")
                        .HasColumnType("int");

                    b.Property<bool?>("EstSimilaire")
                        .HasColumnType("bit");

                    b.Property<string>("Etablissement")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Nom")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<double?>("Note")
                        .HasColumnType("float");

                    b.Property<string>("OptionDiplome")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id")
                        .HasName("PK__Diplomes__3214EC071E66C4A7");

                    b.HasIndex("CandidatureId");

                    b.ToTable("Diplomes");
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CandidatureId")
                        .HasColumnType("int");

                    b.Property<string>("Chemin")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Commentaire")
                        .HasColumnType("text");

                    b.Property<string>("Nom")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Statut")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id")
                        .HasName("PK__Document__3214EC0733786CD9");

                    b.HasIndex("CandidatureId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Experience", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CandidatureId")
                        .HasColumnType("int");

                    b.Property<string>("DomaineActivite")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("NombreAnnee")
                        .HasColumnType("int");

                    b.Property<string>("Poste")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id")
                        .HasName("PK__Experien__3214EC07E68FF620");

                    b.HasIndex("CandidatureId");

                    b.ToTable("Experiences");
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Candidature", b =>
                {
                    b.HasOne("Backoffice_ANCFCC.Models.Administrateur", "Administrateur")
                        .WithMany("Candidatures")
                        .HasForeignKey("AdministrateurId")
                        .HasConstraintName("FK_Candidatures_Administrateur");

                    b.HasOne("Backoffice_ANCFCC.Models.Agent", "Agent")
                        .WithMany("Candidatures")
                        .HasForeignKey("AgentId")
                        .HasConstraintName("FK_Candidatures_Agent");

                    b.HasOne("Backoffice_ANCFCC.Models.Candidat", "Candidats")
                        .WithMany("Candidatures")
                        .HasForeignKey("CandidatsId")
                        .HasConstraintName("FK__Candidatu__Candi__5629CD9C");

                    b.HasOne("Backoffice_ANCFCC.Models.Concour", "Concours")
                        .WithMany("Candidatures")
                        .HasForeignKey("ConcoursId")
                        .HasConstraintName("FK__Candidatu__Conco__571DF1D5");

                    b.Navigation("Administrateur");

                    b.Navigation("Agent");

                    b.Navigation("Candidats");

                    b.Navigation("Concours");
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.ConcoursAttachement", b =>
                {
                    b.HasOne("Backoffice_ANCFCC.Models.Concour", "Concours")
                        .WithMany("ConcoursAttachements")
                        .HasForeignKey("ConcoursId")
                        .HasConstraintName("FK__ConcoursA__Conco__628FA481");

                    b.Navigation("Concours");
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Diplome", b =>
                {
                    b.HasOne("Backoffice_ANCFCC.Models.Candidature", "Candidature")
                        .WithMany("Diplomes")
                        .HasForeignKey("CandidatureId")
                        .HasConstraintName("FK__Diplomes__Candid__59FA5E80");

                    b.Navigation("Candidature");
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Document", b =>
                {
                    b.HasOne("Backoffice_ANCFCC.Models.Candidature", "Candidature")
                        .WithMany("Documents")
                        .HasForeignKey("CandidatureId")
                        .HasConstraintName("FK__Documents__Candi__5CD6CB2B");

                    b.Navigation("Candidature");
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Experience", b =>
                {
                    b.HasOne("Backoffice_ANCFCC.Models.Candidature", "Candidature")
                        .WithMany("Experiences")
                        .HasForeignKey("CandidatureId")
                        .HasConstraintName("FK__Experienc__Candi__5FB337D6");

                    b.Navigation("Candidature");
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Administrateur", b =>
                {
                    b.Navigation("Candidatures");
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Agent", b =>
                {
                    b.Navigation("Candidatures");
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Candidat", b =>
                {
                    b.Navigation("Candidatures");
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Candidature", b =>
                {
                    b.Navigation("Diplomes");

                    b.Navigation("Documents");

                    b.Navigation("Experiences");
                });

            modelBuilder.Entity("Backoffice_ANCFCC.Models.Concour", b =>
                {
                    b.Navigation("Candidatures");

                    b.Navigation("ConcoursAttachements");
                });
#pragma warning restore 612, 618
        }
    }
}
