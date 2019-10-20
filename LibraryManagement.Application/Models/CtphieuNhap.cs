using System;
using System.Collections.Generic;

namespace LibraryManagement.API.Models
{
    public partial class CtphieuNhap
    {
        public int Id { get; set; }
        public int PhieuNhap { get; set; }
        public string Book { get; set; }
        public int SoLuong { get; set; }
        public string TinhTrangSach { get; set; }

        public virtual Sach BookNavigation { get; set; }
        public virtual PhieuNhap PhieuNhapNavigation { get; set; }
    }
}
