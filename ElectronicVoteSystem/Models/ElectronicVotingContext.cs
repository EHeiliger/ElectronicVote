using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ElectronicVoteSystem.Models
{
    public partial class ElectronicVotingContext : IdentityDbContext
    {
        public ElectronicVotingContext()
        {
        }

        public ElectronicVotingContext(DbContextOptions<ElectronicVotingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BallotPaper> BallotPaper { get; set; }
        public virtual DbSet<Candidate> Candidate { get; set; }
        public virtual DbSet<Citizen> Citizen { get; set; }
        public virtual DbSet<Election> Election { get; set; }
        public virtual DbSet<Party> Party { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Vote> Vote { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=LAPTOP-68G0NVSO;DataBase=ElectronicVoting;Trusted_Connection=True;Integrated Security=SSPI;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BallotPaper>(entity =>
            {
                entity.ToTable("ballotPaper");

                entity.HasOne(d => d.Candidate)
                    .WithMany(p => p.BallotPaper)
                    .HasForeignKey(d => d.CandidateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ballotPaper_Candidate");

                entity.HasOne(d => d.Election)
                    .WithMany(p => p.BallotPaper)
                    .HasForeignKey(d => d.ElectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ballotPaper_Election");
            });

            modelBuilder.Entity<Candidate>(entity =>
            {
                entity.Property(e => e.CitizenId)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ProfileAvatar).HasColumnType("text");

                entity.HasOne(d => d.Citizen)
                    .WithMany(p => p.Candidate)
                    .HasForeignKey(d => d.CitizenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Candidate_Citizen");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.Candidate)
                    .HasForeignKey(d => d.PartyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Candidate_Party");
            });

            modelBuilder.Entity<Citizen>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Election>(entity =>
            {
                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.DateInit).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(280)
                    .IsFixedLength();

                entity.Property(e => e.PositionId).HasColumnName("PositionID");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Election)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Election_Position");
            });

            modelBuilder.Entity<Party>(entity =>
            {
                entity.Property(e => e.Color)
                    .HasColumnName("color")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Logo).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(280)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(280)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Vote>(entity =>
            {
                entity.Property(e => e.BallotPaperId).HasColumnName("ballotPaperId");

                entity.Property(e => e.CitizenId)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.BallotPaper)
                    .WithMany(p => p.Vote)
                    .HasForeignKey(d => d.BallotPaperId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vote_ballotPaper");

                entity.HasOne(d => d.Citizen)
                    .WithMany(p => p.Vote)
                    .HasForeignKey(d => d.CitizenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vote_Citizen");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
