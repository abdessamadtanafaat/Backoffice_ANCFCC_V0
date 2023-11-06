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

    public virtual DbSet<AggregatedCounter> AggregatedCounters { get; set; }

    public virtual DbSet<Candidat> Candidats { get; set; }

    public virtual DbSet<Candidature> Candidatures { get; set; }

    public virtual DbSet<Concour> Concours { get; set; }

    public virtual DbSet<ConcoursAttachement> ConcoursAttachements { get; set; }

    public virtual DbSet<Counter> Counters { get; set; }

    public virtual DbSet<Diplome> Diplomes { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Experience> Experiences { get; set; }

    public virtual DbSet<Hash> Hashes { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobParameter> JobParameters { get; set; }

    public virtual DbSet<JobQueue> JobQueues { get; set; }

    public virtual DbSet<List> Lists { get; set; }

    public virtual DbSet<Schema> Schemas { get; set; }

    public virtual DbSet<Server> Servers { get; set; }

    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DB_ANCFCC");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AggregatedCounter>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("PK_HangFire_CounterAggregated");

            entity.ToTable("AggregatedCounter", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_AggregatedCounter_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Candidat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Candidat__3214EC07F3250E92");

            entity.Property(e => e.Adresse)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AdresseArabe)
                .HasMaxLength(255)
                .UseCollation("Arabic_CI_AS");
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
                .HasMaxLength(255)
                .UseCollation("Arabic_CI_AS");
            entity.Property(e => e.Prenom)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PrenomArabe)
                .HasMaxLength(255)
                .UseCollation("Arabic_CI_AS");
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

            entity.Property(e => e.AdminId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AgentId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AssignedTo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AssignedToAdmin)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CommentaireDoc)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.CommentaireExp)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.CommentaireFormation)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.CommentaireInfoPerso)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.DateModification).HasColumnType("datetime");
            entity.Property(e => e.DatePostulation).HasColumnType("date");
            entity.Property(e => e.Reference).HasDefaultValueSql("('R'+right(CONVERT([nvarchar](20),abs(checksum(newid()))),(6)))");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken()
                .IsRequired(false); 
            entity.Property(e => e.StatusGlobal).HasColumnName("statusGlobal");
            entity.Property(e => e.TreatedByAdmin)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TreatedByAgent)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Candidats).WithMany(p => p.Candidatures)
                .HasForeignKey(d => d.CandidatsId)
                .HasConstraintName("FK__Candidatu__Candi__5629CD9C");

            entity.HasOne(d => d.Concours).WithMany(p => p.Candidatures)
                .HasForeignKey(d => d.ConcoursId)
                .HasConstraintName("FK__Candidatu__Conco__571DF1D5");

            entity.HasOne(d => d.User).WithMany(p => p.Candidatures)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Candidature_UserId");
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

        modelBuilder.Entity<Counter>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Id }).HasName("PK_HangFire_Counter");

            entity.ToTable("Counter", "HangFire");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
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

        modelBuilder.Entity<Hash>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Field }).HasName("PK_HangFire_Hash");

            entity.ToTable("Hash", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Hash_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Field).HasMaxLength(100);
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HangFire_Job");

            entity.ToTable("Job", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Job_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.HasIndex(e => e.StateName, "IX_HangFire_Job_StateName").HasFilter("([StateName] IS NOT NULL)");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            entity.Property(e => e.StateName).HasMaxLength(20);
        });

        modelBuilder.Entity<JobParameter>(entity =>
        {
            entity.HasKey(e => new { e.JobId, e.Name }).HasName("PK_HangFire_JobParameter");

            entity.ToTable("JobParameter", "HangFire");

            entity.Property(e => e.Name).HasMaxLength(40);

            entity.HasOne(d => d.Job).WithMany(p => p.JobParameters)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK_HangFire_JobParameter_Job");
        });

        modelBuilder.Entity<JobQueue>(entity =>
        {
            entity.HasKey(e => new { e.Queue, e.Id }).HasName("PK_HangFire_JobQueue");

            entity.ToTable("JobQueue", "HangFire");

            entity.Property(e => e.Queue).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.FetchedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<List>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Id }).HasName("PK_HangFire_List");

            entity.ToTable("List", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_List_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Schema>(entity =>
        {
            entity.HasKey(e => e.Version).HasName("PK_HangFire_Schema");

            entity.ToTable("Schema", "HangFire");

            entity.Property(e => e.Version).ValueGeneratedNever();
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HangFire_Server");

            entity.ToTable("Server", "HangFire");

            entity.HasIndex(e => e.LastHeartbeat, "IX_HangFire_Server_LastHeartbeat");

            entity.Property(e => e.Id).HasMaxLength(200);
            entity.Property(e => e.LastHeartbeat).HasColumnType("datetime");
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Value }).HasName("PK_HangFire_Set");

            entity.ToTable("Set", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Set_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.HasIndex(e => new { e.Key, e.Score }, "IX_HangFire_Set_Score");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Value).HasMaxLength(256);
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => new { e.JobId, e.Id }).HasName("PK_HangFire_State");

            entity.ToTable("State", "HangFire");

            entity.HasIndex(e => e.CreatedAt, "IX_HangFire_State_CreatedAt");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.Reason).HasMaxLength(100);

            entity.HasOne(d => d.Job).WithMany(p => p.States)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK_HangFire_State_Job");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Administ__5AAEDD3CEBBE43CC");

            entity.HasIndex(e => e.Email, "UQ_Email").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastLoginAttemptDate).HasColumnType("datetime");
            entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.LockoutTimestamp).HasColumnType("datetime");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PasswordResetToken).HasMaxLength(255);
            entity.Property(e => e.Prenom)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResetTokenExpires).HasColumnType("datetime");
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.UserUniqueCode)
                .HasMaxLength(10)
                .HasDefaultValueSql("('U'+right(CONVERT([nvarchar](20),abs(checksum(newid()))),(6)))");
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.VerificationToken).HasMaxLength(255);
            entity.Property(e => e.VerifiedAt).HasColumnType("datetime");
        });
        modelBuilder.HasSequence("UserIdSequence");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
