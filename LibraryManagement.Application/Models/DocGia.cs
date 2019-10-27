using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Models
{
    public partial class DocGia
    {
        public DocGia()
        {
            PhieuMuon = new HashSet<PhieuMuon>();
        }

        public int Id { get; set; }
        public string TenDg { get; set; }
        public string GioiTinh { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime NgaySinh { get; set; }
        public string Cmnd { get; set; }
        public string DiaChi { get; set; }
        public string Sdt { get; set; }
        public int? SoLanViPham { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime NgayDangKy { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? TrangThai { get; set; }

        public virtual ICollection<PhieuMuon> PhieuMuon { get; set; }
    }
}
