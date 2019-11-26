using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày mượn")]
        public DateTime NgayMuon { get; set; }

        [Display(Name = "Số sách mượn")]
        public int? TongSachMuon { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Hạn phải trả")]
        public DateTime HanTra { get; set; }

        [Display(Name = "Đã trả sách?")]
        public bool DaTra { get; set; }

        [Display(Name = "Trạng thái")]
        public int? TrangThai { get; set; }

        public virtual DocGia MaDgNavigation { get; set; }
        public virtual NhanVien MaNvNavigation { get; set; }

        [JsonIgnore]
        public virtual ICollection<CtphieuMuon> CtphieuMuon { get; set; }
    }
}
