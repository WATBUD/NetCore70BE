using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NetCore60.Models;

public partial class RndatingDbContext : DbContext
{
    private readonly string _connectionString="";

    public RndatingDbContext(string? connectionString)
    {
        if(connectionString!=null) _connectionString = connectionString;
    }

    public RndatingDbContext(DbContextOptions<RndatingDbContext> options, string connectionString)
        : base(options)
    {
        if (connectionString != null) _connectionString = connectionString;
    }

    public virtual DbSet<AllTag> AllTags { get; set; }

    public virtual DbSet<AllTagsGroup> AllTagsGroups { get; set; }

    public virtual DbSet<RecordLogTable> RecordLogTables { get; set; }

    public virtual DbSet<RequestLog> RequestLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    public virtual DbSet<VTagGroupDetail> VTagGroupDetails { get; set; }

    public virtual DbSet<VUsersDetail> VUsersDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(_connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.1.0-mysql"));
    }


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

        modelBuilder.Entity<RecordLogTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("record_log_table");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataText)
                .HasMaxLength(200)
                .HasColumnName("data_text");
        });

        modelBuilder.Entity<RequestLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("request_logs");

            entity.Property(e => e.ClientIp).HasMaxLength(45);
            entity.Property(e => e.Method).HasMaxLength(10);
            entity.Property(e => e.Path).HasMaxLength(255);
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
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
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity.HasKey(e => e.UdUserId).HasName("PRIMARY");

            entity.ToTable("user_detail");

            entity.Property(e => e.UdUserId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ud_user_id");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
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
            entity.Property(e => e.UserHasTag)
                .HasColumnType("json")
                .HasColumnName("user_has_tag");

            entity.HasOne(d => d.UdUser).WithOne(p => p.UserDetail)
                .HasForeignKey<UserDetail>(d => d.UdUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_userdetail_user");
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

        modelBuilder.Entity<VUsersDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V_UsersDetail");

            entity.Property(e => e.Account)
                .HasMaxLength(100)
                .HasColumnName("account");
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
            entity.Property(e => e.UdUserId).HasColumnName("ud_user_id");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserHasTag)
                .HasColumnType("json")
                .HasColumnName("user_has_tag");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
