using System;
using System.Collections.Generic;

namespace LibraryManagement.API.Models
{
    public partial class PhieuNhap
    {
        public PhieuNhap()
        {
            CtphieuNhap = new HashSet<CtphieuNhap>();
        }

        public int Id { get; set; }
        public DateTime NgayNhap { get; set; }
        public int? SoLuong { get; set; }
        public string NhaCungCap { get; set; }
        public int? NhanVienNhap { get; set; }

        public virtual NhanVien NhanVienNhapNavigation { get; set; }
        public virtual ICollection<CtphieuNhap> CtphieuNhap { get; set; }
    }
}
