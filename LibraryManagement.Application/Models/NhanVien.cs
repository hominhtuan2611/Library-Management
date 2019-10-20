using System;
using System.Collections.Generic;

namespace LibraryManagement.API.Models
{
    public partial class NhanVien
    {
        public NhanVien()
        {
            PhieuMuon = new HashSet<PhieuMuon>();
            PhieuNhap = new HashSet<PhieuNhap>();
        }

        public int Id { get; set; }
        public string TenNv { get; set; }
        public string GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Cmnd { get; set; }
        public string DiaChi { get; set; }
        public string Sdt { get; set; }
        public string ViTri { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? TrangThai { get; set; }

        public virtual ICollection<PhieuMuon> PhieuMuon { get; set; }
        public virtual ICollection<PhieuNhap> PhieuNhap { get; set; }
    }
}
