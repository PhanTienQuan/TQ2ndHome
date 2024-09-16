using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TQuanHome.Areas.Identity.Data;

public partial class TQuanHomeContext : IdentityDbContext<TQuanHomeUser>
{
    public TQuanHomeContext()
    {
    }

    public TQuanHomeContext(DbContextOptions<TQuanHomeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<PostInfo> PostInfos { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

    // DbSet cho SavedPost
    public DbSet<SavedPost> SavedPosts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=QUANPHAN;Initial Catalog=TQuanHome;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.DistrictId).HasName("PK__District__85FDA4A634D7C274");

            entity.Property(e => e.DistrictId)
                .ValueGeneratedNever()
                .HasColumnName("DistrictID");
            entity.Property(e => e.DistrictName).HasMaxLength(255);
            entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");

            entity.HasOne(d => d.Province).WithMany(p => p.Districts)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("FK__Districts__Provi__693CA210");
        });

        modelBuilder.Entity<PostInfo>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__PostInfo__AA12603894F64089");

            entity.ToTable("PostInfo");

            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.AddressDetail).HasMaxLength(450);
            entity.Property(e => e.DistrictId).HasColumnName("DistrictID");
            entity.Property(e => e.PostDate).HasColumnType("datetime");
            entity.Property(e => e.PostTitle).HasMaxLength(450);
            entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");
            entity.Property(e => e.UserName).HasMaxLength(256);
            entity.Property(e => e.WardId).HasColumnName("WardID");

            entity.HasOne(d => d.District).WithMany(p => p.PostInfos)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK__PostInfo__Distri__6FE99F9F");

            entity.HasOne(d => d.Province).WithMany(p => p.PostInfos)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("FK__PostInfo__Provin__6EF57B66");

            entity.HasOne(d => d.Ward).WithMany(p => p.PostInfos)
                .HasForeignKey(d => d.WardId)
                .HasConstraintName("FK__PostInfo__WardID__70DDC3D8");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.ProvinceId).HasName("PK__Province__FD0A6FA3A0003DFC");

            entity.Property(e => e.ProvinceId)
                .ValueGeneratedNever()
                .HasColumnName("ProvinceID");
            entity.Property(e => e.ProvinceName).HasMaxLength(255);
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasKey(e => e.WardId).HasName("PK__Wards__C6BD9BEADC878773");

            entity.Property(e => e.WardId)
                .ValueGeneratedNever()
                .HasColumnName("WardID");
            entity.Property(e => e.DistrictId).HasColumnName("DistrictID");
            entity.Property(e => e.WardName).HasMaxLength(255);

            entity.HasOne(d => d.District).WithMany(p => p.Wards)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK__Wards__DistrictI__6C190EBB");
        });
        // Cấu hình khóa chính cho SavedPost
        modelBuilder.Entity<SavedPost>()
            .HasKey(sp => new { sp.PostId, sp.UserName });

        // Định nghĩa quan hệ giữa SavedPost và PostInfo
        modelBuilder.Entity<SavedPost>()
            .HasOne(sp => sp.PostInfo)
            .WithOne()
            .HasForeignKey<SavedPost>(sp => sp.PostId);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
