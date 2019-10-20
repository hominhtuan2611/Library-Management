using System;
using System.Collections.Generic;

namespace LibraryManagement.API.Models
{
    public partial class CtphieuMuon
    {
        public int Id { get; set; }
        public int PhieuMuon { get; set; }
        public string Book { get; set; }
        public int SoLuong { get; set; }
        public DateTime? NgayMuon { get; set; }
        public DateTime? NgayTra { get; set; }
        public string TinhTrangSach { get; set; }

        public virtual Sach BookNavigation { get; set; }
        public virtual PhieuMuon PhieuMuonNavigation { get; set; }
    }
}
