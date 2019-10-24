using System;
using System.Collections.Generic;

namespace LibraryManagement.API.Models
{
    public partial class Sach
    {
        public Sach()
        {
            CtphieuMuon = new HashSet<CtphieuMuon>();
            CtphieuNhap = new HashSet<CtphieuNhap>();
        }

        public string Id { get; set; }
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public string NhaXuatBan { get; set; }
        public int? NamXuatBan { get; set; }
        public int? TongSoTrang { get; set; }
        public string TomTat { get; set; }
        public int? LoaiSach { get; set; }
        public int? SoLuong { get; set; }
        public string HinhAnh { get; set; }
        public bool? TrangThai { get; set; }

        public virtual LoaiSach LoaiSachNavigation { get; set; }
        public virtual ICollection<CtphieuMuon> CtphieuMuon { get; set; }
        public virtual ICollection<CtphieuNhap> CtphieuNhap { get; set; }
    }
}
