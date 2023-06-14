using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebLevent.Models
{
    public partial class SanPham
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? NhanVienId { get; set; }
        /*[StringLength(255, ErrorMessage = "The full name field should have a maximum of 255 characters")]*/
        /*[Required]
        [MinLength(4)]*/
        public int Price { get; set; }
        public string? Mota { get; set; }
        public string? Hinh { get; set; }
        public int? SoLgTn { get; set; }
        public DateTime? DateTimeCreate { get; set; }
        public DateTime? DateTimeEdit { get; set; }
        public int? LoaiId { get; set; }

        public virtual LoaiSanPham? Loai { get; set; }
    }
}
