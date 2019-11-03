using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Models
{
    public partial class PhieuNhap
    {
        public PhieuNhap()
        {
            CtphieuNhap = new HashSet<CtphieuNhap>();
        }

        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày mượn")]
        public DateTime NgayNhap { get; set; }

        [Display(Name = "Số lượng nhập")]
        public int? SoLuong { get; set; }

        [StringLength(50)]
        [Display(Name = "Nhà cung cấp")]
        public string NhaCungCap { get; set; }

        [Display(Name = "Nhân viên nhập")]
        public int? NhanVienNhap { get; set; }

        [Display(Name = "Trạng thái")]
        public int? TrangThai { get; set; }

        public virtual NhanVien NhanVienNhapNavigation { get; set; }
        public virtual ICollection<CtphieuNhap> CtphieuNhap { get; set; }
    }
}
