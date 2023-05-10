#nullable disable

using AspTest.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AspTest.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext()
    {
    }

    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }

    public virtual DbSet<UserState> UserStates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Name=ConnectionString");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_id");

            entity.ToTable("user");

            entity.HasIndex(e => e.Login, "user_login_uq").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.Login)
                .HasMaxLength(32)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(128)
                .HasColumnName("password");
            entity.Property(e => e.UserGroupId).HasColumnName("user_group_id");
            entity.Property(e => e.UserStateId).HasColumnName("user_state_id");

            entity.HasOne(d => d.UserGroup).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_user_group_user_group_id");

            entity.HasOne(d => d.UserState).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserStateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_user_state_user_state_id");
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_group_id");

            entity.ToTable("user_group");

            entity.HasIndex(e => e.Code, "user_group_code_uq").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(32)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasColumnName("description");
        });

        modelBuilder.Entity<UserState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_state_id");

            entity.ToTable("user_state");

            entity.HasIndex(e => e.Code, "user_state_code_uq").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(32)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasColumnName("description");
        });
    }
}