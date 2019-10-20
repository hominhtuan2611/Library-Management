using System;
using System.Collections.Generic;

namespace LibraryManagement.API.Models
{
    public partial class KhachHang
    {
        public KhachHang()
        {
            PhieuMuon = new HashSet<PhieuMuon>();
        }

        public int Id { get; set; }
        public string TenKh { get; set; }
        public string GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Cmnd { get; set; }
        public string DiaChi { get; set; }
        public string Sdt { get; set; }
        public int? SoLanViPham { get; set; }
        public DateTime? NgayDangKy { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? TrangThai { get; set; }

        public virtual ICollection<PhieuMuon> PhieuMuon { get; set; }
    }
}
