using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Models
{
    public partial class Sach
    {
        public Sach()
        {
            CtphieuMuon = new HashSet<CtphieuMuon>();
            CtphieuNhap = new HashSet<CtphieuNhap>();
        }

        [Key]
        [StringLength(20)]
        [Display(Name = "Mã sách")]
        public string Id { get; set; }

        [StringLength(100), Required(ErrorMessage = "Vui lòng nhập tên sách")]
        [Display(Name = "Tên sách")]
        public string TenSach { get; set; }

        [StringLength(30), Required(ErrorMessage = "Vui lòng nhập  tên tác giả")]
        [Display(Name = "Tên tác giả")]
        public string TacGia { get; set; }

        [StringLength(100)]
        [Display(Name = "Nhà xuất bản")]
        public string NhaXuatBan { get; set; }

        [Display(Name = "Năm xuất bản")]
        public int? NamXuatBan { get; set; }

        [Display(Name = "Tổng số trang")]
        public int? TongSoTrang { get; set; }

        [Display(Name = "Tóm tắt nội dung")]
        public string TomTat { get; set; }

        [Display(Name = "Thể loại")]
        public int? LoaiSach { get; set; }

        [Display(Name = "Số lượng")]
        public int? SoLuong { get; set; }

        [Display(Name = "Hình ảnh")]
        public string HinhAnh { get; set; }

        [Display(Name = "Trạng thái")]
        public bool? TrangThai { get; set; }

        public virtual LoaiSach LoaiSachNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<CtphieuMuon> CtphieuMuon { get; set; }
        [JsonIgnore]
        public virtual ICollection<CtphieuNhap> CtphieuNhap { get; set; }
    }
}
