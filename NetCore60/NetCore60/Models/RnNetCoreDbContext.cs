using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NetCore60.Models;

public partial class RnNetCoreDbContext : DbContext
{
    public RnNetCoreDbContext()
    {
    }

    public RnNetCoreDbContext(DbContextOptions<RnNetCoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EfmigrationsHistory> EfmigrationsHistories { get; set; }

    public virtual DbSet<RecordLogTable> RecordLogTables { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserStatus> UserStatuses { get; set; }

    public virtual DbSet<VUsersDetail> VUsersDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=127.0.0.1;database=RN_NetCoreDB;user=louis;password=123456", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.1.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<EfmigrationsHistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__EFMigrationsHistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<RecordLogTable>(entity =>
        {
            entity.HasKey(e => e.DataText).HasName("PRIMARY");

            entity.ToTable("RecordLogTable");

            entity.Property(e => e.DataText).HasMaxLength(200);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.Username, "Username").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IsBanned).HasDefaultValueSql("'0'");
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<UserStatus>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("UserStatus");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.IsBanned).HasDefaultValueSql("'0'");

            entity.HasOne(d => d.User).WithOne(p => p.UserStatus)
                .HasForeignKey<UserStatus>(d => d.UserId)
                .HasConstraintName("UserStatus_ibfk_1");
        });

        modelBuilder.Entity<VUsersDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V_UsersDetail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IsBanned).HasDefaultValueSql("'0'");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
