using Newtonsoft.Json;
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

        [StringLength(30), MinLength(5, ErrorMessage = "Tên tối thiểu chứa 5 kí tự"), MaxLength(40, ErrorMessage = "Tên vượt quá độ dài cho phép"), Required(ErrorMessage = "Vui lòng nhập tên")]
        [Display(Name = "Họ tên")]
        public string TenDg { get; set; }

        [StringLength(10)]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày sinh")]
        public DateTime NgaySinh { get; set; }

        [StringLength(12)]
        [Display(Name = "Số CMND")]
        public string Cmnd { get; set; }

        [StringLength(50), MaxLength(50, ErrorMessage = "Địa chỉ vượt quá độ dài cho phép")]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [StringLength(11), MinLength(10, ErrorMessage = "Số điện thoại tối thiểu chứa 10 kí tự"), MaxLength(11, ErrorMessage = "Số điện thoại vượt quá độ dài cho phép")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string Sdt { get; set; }

        [Display(Name = "Số lần vi phạm")]
        public int? SoLanViPham { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày đăng ký")]
        public DateTime NgayDangKy { get; set; }

        [StringLength(50), MaxLength(50, ErrorMessage = "Địa chỉ mail vượt quá độ dài cho phép"), Required(ErrorMessage = "Vui lòng nhập vào địa chỉ mail")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Sai định dạng! Địa chỉ mail ví dụ: tanht1997@gmail.com")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Tài khoản")]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Trạng thái")]
        public bool? TrangThai { get; set; }

        [JsonIgnore]
        public virtual ICollection<PhieuMuon> PhieuMuon { get; set; }
    }
}
