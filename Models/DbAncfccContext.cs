using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Backoffice_ANCFCC.Models;

public partial class DbAncfccContext : DbContext
{
    public DbAncfccContext()
    {
    }

    public DbAncfccContext(DbContextOptions<DbAncfccContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Candidat> Candidats { get; set; }

    public virtual DbSet<Candidature> Candidatures { get; set; }

    public virtual DbSet<Concour> Concours { get; set; }

    public virtual DbSet<ConcoursAttachement> ConcoursAttachements { get; set; }

    public virtual DbSet<Diplome> Diplomes { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Experience> Experiences { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DB_ANCFCC");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Candidat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Candidat__3214EC07F3250E92");

            entity.Property(e => e.Adresse)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AdresseArabe)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Cin)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CodePostal)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateNaissance).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Genre)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Matrimoniale)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NomArabe)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Prenom)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PrenomArabe)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telephone1)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Telephone2)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Ville)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Candidature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Candidat__3214EC0725A922E0");

            entity.Property(e => e.CommentaireDoc)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CommentaireExp)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CommentaireInfoPerso).HasColumnType("text");
            entity.Property(e => e.DateModification).HasColumnType("datetime");
            entity.Property(e => e.DatePostulation).HasColumnType("date");
            entity.Property(e => e.Reference)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatutDoc)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.StatutExp)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.StatutFormation).HasMaxLength(50);
            entity.Property(e => e.StatutInfoPerso)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Candidats).WithMany(p => p.Candidatures)
                .HasForeignKey(d => d.CandidatsId)
                .HasConstraintName("FK__Candidatu__Candi__5629CD9C");

            entity.HasOne(d => d.Concours).WithMany(p => p.Candidatures)
                .HasForeignKey(d => d.ConcoursId)
                .HasConstraintName("FK__Candidatu__Conco__571DF1D5");
        });

        modelBuilder.Entity<Concour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Concours__3214EC076D49CFE5");

            entity.Property(e => e.DateExpiration).HasColumnType("date");
            entity.Property(e => e.DatePublication).HasColumnType("date");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Diplome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Domaine)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ConcoursAttachement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Concours__3214EC07AC9DE213");

            entity.Property(e => e.Chemin)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Concours).WithMany(p => p.ConcoursAttachements)
                .HasForeignKey(d => d.ConcoursId)
                .HasConstraintName("FK__ConcoursA__Conco__628FA481");
        });

        modelBuilder.Entity<Diplome>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Diplomes__3214EC071E66C4A7");

            entity.Property(e => e.AutreDiplome)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Etablissement)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OptionDiplome)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Candidature).WithMany(p => p.Diplomes)
                .HasForeignKey(d => d.CandidatureId)
                .HasConstraintName("FK__Diplomes__Candid__59FA5E80");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC0733786CD9");

            entity.Property(e => e.Chemin)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Commentaire).HasColumnType("text");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Statut)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Candidature).WithMany(p => p.Documents)
                .HasForeignKey(d => d.CandidatureId)
                .HasConstraintName("FK__Documents__Candi__5CD6CB2B");
        });

        modelBuilder.Entity<Experience>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Experien__3214EC07E68FF620");

            entity.Property(e => e.DomaineActivite)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Poste)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Candidature).WithMany(p => p.Experiences)
                .HasForeignKey(d => d.CandidatureId)
                .HasConstraintName("FK__Experienc__Candi__5FB337D6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
