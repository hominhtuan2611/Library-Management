using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LibraryManagement.API.Models
{
    public partial class LibraryDBContext : DbContext
    {
        public LibraryDBContext()
        {
        }

        public LibraryDBContext(DbContextOptions<LibraryDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CtphieuMuon> CtphieuMuon { get; set; }
        public virtual DbSet<CtphieuNhap> CtphieuNhap { get; set; }
        public virtual DbSet<KhachHang> KhachHang { get; set; }
        public virtual DbSet<LoaiSach> LoaiSach { get; set; }
        public virtual DbSet<NhanVien> NhanVien { get; set; }
        public virtual DbSet<PhieuMuon> PhieuMuon { get; set; }
        public virtual DbSet<PhieuNhap> PhieuNhap { get; set; }
        public virtual DbSet<Sach> Sach { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Data Source=DESKTOP-7MFA92E\\SQLEXPRESS;Initial Catalog=LibraryDB;Trusted_Connection=True;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<CtphieuMuon>(entity =>
            {
                entity.ToTable("CTPhieuMuon");

                entity.Property(e => e.Book)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NgayMuon).HasColumnType("date");

                entity.Property(e => e.NgayTra).HasColumnType("date");

                entity.Property(e => e.TinhTrangSach).HasMaxLength(50);

                entity.HasOne(d => d.BookNavigation)
                    .WithMany(p => p.CtphieuMuon)
                    .HasForeignKey(d => d.Book)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CTPhieuMuon_Books");

                entity.HasOne(d => d.PhieuMuonNavigation)
                    .WithMany(p => p.CtphieuMuon)
                    .HasForeignKey(d => d.PhieuMuon)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CTPhieuMuon_PhieuMuon");
            });

            modelBuilder.Entity<CtphieuNhap>(entity =>
            {
                entity.ToTable("CTPhieuNhap");

                entity.Property(e => e.Book)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TinhTrangSach).HasMaxLength(50);

                entity.HasOne(d => d.BookNavigation)
                    .WithMany(p => p.CtphieuNhap)
                    .HasForeignKey(d => d.Book)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CTPhieuNhap_Books");

                entity.HasOne(d => d.PhieuNhapNavigation)
                    .WithMany(p => p.CtphieuNhap)
                    .HasForeignKey(d => d.PhieuNhap)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CTPhieuNhap_PhieuNhap");
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.Property(e => e.Cmnd)
                    .IsRequired()
                    .HasColumnName("CMND")
                    .HasMaxLength(12);

                entity.Property(e => e.DiaChi).HasMaxLength(50);

                entity.Property(e => e.GioiTinh)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.NgayDangKy).HasColumnType("date");

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.Sdt)
                    .HasColumnName("SDT")
                    .HasMaxLength(11);

                entity.Property(e => e.SoLanViPham).HasDefaultValueSql("((0))");

                entity.Property(e => e.TenKh)
                    .IsRequired()
                    .HasColumnName("TenKH")
                    .HasMaxLength(30);

                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LoaiSach>(entity =>
            {
                entity.Property(e => e.TenLoai)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.Property(e => e.Cmnd)
                    .IsRequired()
                    .HasColumnName("CMND")
                    .HasMaxLength(12);

                entity.Property(e => e.DiaChi).HasMaxLength(50);

                entity.Property(e => e.GioiTinh)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.Sdt)
                    .HasColumnName("SDT")
                    .HasMaxLength(11);

                entity.Property(e => e.TenNv)
                    .IsRequired()
                    .HasColumnName("TenNV")
                    .HasMaxLength(30);

                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ViTri).HasMaxLength(30);
            });

            modelBuilder.Entity<PhieuMuon>(entity =>
            {
                entity.Property(e => e.HanTra).HasColumnType("date");

                entity.Property(e => e.MaKh).HasColumnName("MaKH");

                entity.Property(e => e.MaNv).HasColumnName("MaNV");

                entity.Property(e => e.NgayMuon).HasColumnType("date");

                entity.HasOne(d => d.MaKhNavigation)
                    .WithMany(p => p.PhieuMuon)
                    .HasForeignKey(d => d.MaKh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PhieuMuon_KhachHang");

                entity.HasOne(d => d.MaNvNavigation)
                    .WithMany(p => p.PhieuMuon)
                    .HasForeignKey(d => d.MaNv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PhieuMuon_NhanVien");
            });

            modelBuilder.Entity<PhieuNhap>(entity =>
            {
                entity.Property(e => e.NgayNhap).HasColumnType("date");

                entity.Property(e => e.NhaCungCap).HasMaxLength(50);

                entity.HasOne(d => d.NhanVienNhapNavigation)
                    .WithMany(p => p.PhieuNhap)
                    .HasForeignKey(d => d.NhanVienNhap)
                    .HasConstraintName("FK_PhieuNhap_NhanVien");
            });

            modelBuilder.Entity<Sach>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.NhaXuatBan).HasMaxLength(100);

                entity.Property(e => e.TacGia)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.TenSach)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.LoaiSachNavigation)
                    .WithMany(p => p.Sach)
                    .HasForeignKey(d => d.LoaiSach)
                    .HasConstraintName("FK_Books_ToBookTypes");
            });
        }
    }
}
