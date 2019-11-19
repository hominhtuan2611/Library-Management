using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Models
{
    public partial class LoaiSach
    {
        public LoaiSach()
        {
            Sach = new HashSet<Sach>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name = "Thể loại")]
        public string TenLoai { get; set; }

        [JsonIgnore]
        public virtual ICollection<Sach> Sach { get; set; }
    }
}
