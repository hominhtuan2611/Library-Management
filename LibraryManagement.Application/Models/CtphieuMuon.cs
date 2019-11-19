using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Models
{
    public partial class CtphieuMuon
    {
        public int Id { get; set; }

        [Display(Name = "Mã phiếu mượn")]
        public int PhieuMuon { get; set; }

        [Display(Name = "Tên sách")]
        public string Book { get; set; }

        [Range(1, 100, ErrorMessage = "Số lượng không hợp lệ")]
        [Required(ErrorMessage = "Vui lòng nhập vào số lượng sách")]
        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }

        [Display(Name = "Tình trạng sách")]
        public string TinhTrangSach { get; set; }

        public virtual Sach BookNavigation { get; set; }
        public virtual PhieuMuon PhieuMuonNavigation { get; set; }
    }
}
