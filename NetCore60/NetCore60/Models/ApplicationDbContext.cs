using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NetCore60.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AllTag> AllTags { get; set; }

    public virtual DbSet<AllTagsGroup> AllTagsGroups { get; set; }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PrismaMigration> PrismaMigrations { get; set; }

    public virtual DbSet<RecordLogTable> RecordLogTables { get; set; }

    public virtual DbSet<RequestLog> RequestLogs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserStock> UserStocks { get; set; }

    public virtual DbSet<VTagGroupDetail> VTagGroupDetails { get; set; }

    public virtual DbSet<VUserRolePermission> VUserRolePermissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=127.0.0.1;database=RNDatingDB;user=louis005;password=louis005", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.1.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<AllTag>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("all_tags");

            entity.HasIndex(e => e.TagId, "all_tags_UN").IsUnique();

            entity.Property(e => e.TagGroupId).HasColumnName("tag_group_id");
            entity.Property(e => e.TagId)
                .HasDefaultValueSql("'2'")
                .HasColumnName("tag_id");
            entity.Property(e => e.TagName)
                .HasMaxLength(40)
                .HasDefaultValueSql("'empty'")
                .HasColumnName("tag_Name");
        });

        modelBuilder.Entity<AllTagsGroup>(entity =>
        {
            entity.HasKey(e => e.TagGroupId).HasName("PRIMARY");

            entity.ToTable("all_tags_group");

            entity.Property(e => e.TagGroupId)
                .ValueGeneratedNever()
                .HasColumnName("tag_group_id");
            entity.Property(e => e.TagGroupName)
                .HasMaxLength(10)
                .HasColumnName("tag_group_name");
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PRIMARY");

            entity.ToTable("chat_messages");

            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.ReceiverId).HasColumnName("receiver_id");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");
            entity.Property(e => e.TimeStamp)
                .HasColumnType("timestamp")
                .HasColumnName("time_stamp");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("permissions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(50)
                .HasColumnName("permission_name");
        });

        modelBuilder.Entity<PrismaMigration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("_prisma_migrations")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .HasColumnName("id");
            entity.Property(e => e.AppliedStepsCount).HasColumnName("applied_steps_count");
            entity.Property(e => e.Checksum)
                .HasMaxLength(64)
                .HasColumnName("checksum");
            entity.Property(e => e.FinishedAt)
                .HasColumnType("datetime(3)")
                .HasColumnName("finished_at");
            entity.Property(e => e.Logs)
                .HasColumnType("text")
                .HasColumnName("logs");
            entity.Property(e => e.MigrationName)
                .HasMaxLength(255)
                .HasColumnName("migration_name");
            entity.Property(e => e.RolledBackAt)
                .HasColumnType("datetime(3)")
                .HasColumnName("rolled_back_at");
            entity.Property(e => e.StartedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP(3)")
                .HasColumnType("datetime(3)")
                .HasColumnName("started_at");
        });

        modelBuilder.Entity<RecordLogTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("record_log_table");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.DataText)
                .HasMaxLength(200)
                .HasColumnName("data_text");
        });

        modelBuilder.Entity<RequestLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("request_logs");

            entity.Property(e => e.BackendLanguage)
                .HasMaxLength(50)
                .HasColumnName("backend_language");
            entity.Property(e => e.ClientIp)
                .HasMaxLength(45)
                .HasColumnName("client_ip");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Method)
                .HasMaxLength(10)
                .HasColumnName("method");
            entity.Property(e => e.Path)
                .HasMaxLength(255)
                .HasColumnName("path");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");

            entity.HasMany(d => d.Permissions).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolePermission",
                    r => r.HasOne<Permission>().WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("role_permissions_ibfk_2"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("role_permissions_ibfk_1"),
                    j =>
                    {
                        j.HasKey("RoleId", "PermissionId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("role_permissions");
                        j.HasIndex(new[] { "PermissionId" }, "permission_id");
                        j.IndexerProperty<int>("RoleId").HasColumnName("role_id");
                        j.IndexerProperty<int>("PermissionId").HasColumnName("permission_id");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Account, "Account").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Account)
                .HasMaxLength(100)
                .HasColumnName("account");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .HasColumnName("avatar");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.Interests)
                .HasColumnType("text")
                .HasColumnName("interests");
            entity.Property(e => e.IsBanned)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_banned");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.LookingFor)
                .HasColumnType("enum('Friendship','Dating','Long-term Relationship','Other')")
                .HasColumnName("looking_for");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.PersonalDescription)
                .HasColumnType("text")
                .HasColumnName("personal_description");
            entity.Property(e => e.PrivacySettings)
                .HasColumnType("json")
                .HasColumnName("privacy_settings");
            entity.Property(e => e.ProfilePicture)
                .HasMaxLength(255)
                .HasColumnName("profile_picture");
            entity.Property(e => e.RelationshipStatus)
                .HasColumnType("enum('Single','Married','Divorced','Other')")
                .HasColumnName("relationship_status");
            entity.Property(e => e.SocialLinks)
                .HasColumnType("json")
                .HasColumnName("social_links");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserHasTag)
                .HasColumnType("json")
                .HasColumnName("user_has_tag");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("user_roles_ibfk_2"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("user_roles_ibfk_1"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("user_roles");
                        j.HasIndex(new[] { "RoleId" }, "role_id");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("RoleId").HasColumnName("role_id");
                    });
        });

        modelBuilder.Entity<UserStock>(entity =>
        {
            entity.HasKey(e => e.Index).HasName("PRIMARY");

            entity.ToTable("user_stock");

            entity.HasIndex(e => new { e.StockId, e.UserId }, "user_stock_key").IsUnique();

            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.IsBlocked)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_blocked");
            entity.Property(e => e.Note)
                .HasMaxLength(16)
                .HasColumnName("note");
            entity.Property(e => e.StockId)
                .HasMaxLength(6)
                .HasColumnName("stock_id");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId)
                .HasMaxLength(16)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<VTagGroupDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V_TagGroupDetail");

            entity.Property(e => e.TagGroupId).HasColumnName("tag_group_id");
            entity.Property(e => e.TagGroupName)
                .HasMaxLength(10)
                .HasColumnName("tag_group_name");
            entity.Property(e => e.TagId)
                .HasDefaultValueSql("'2'")
                .HasColumnName("tag_id");
            entity.Property(e => e.TagName)
                .HasMaxLength(40)
                .HasDefaultValueSql("'empty'")
                .HasColumnName("tag_Name");
        });

        modelBuilder.Entity<VUserRolePermission>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_user_role_permissions");

            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(50)
                .HasColumnName("permission_name");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
