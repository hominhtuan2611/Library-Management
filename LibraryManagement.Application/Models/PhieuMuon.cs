using System;
using System.Collections.Generic;

namespace LibraryManagement.API.Models
{
    public partial class PhieuMuon
    {
        public PhieuMuon()
        {
            CtphieuMuon = new HashSet<CtphieuMuon>();
        }

        public int Id { get; set; }
        public int MaDg { get; set; }
        public int MaNv { get; set; }
        public DateTime NgayMuon { get; set; }
        public int? TongSachMuon { get; set; }
        public DateTime HanTra { get; set; }
        public bool DaTra { get; set; }
        public int? TrangThai { get; set; }

        public virtual DocGia MaDgNavigation { get; set; }
        public virtual NhanVien MaNvNavigation { get; set; }
        public virtual ICollection<CtphieuMuon> CtphieuMuon { get; set; }
    }
}
