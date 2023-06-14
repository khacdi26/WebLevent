using System;
using System.Collections.Generic;

namespace WebLevent.Models
{
    public partial class LoaiSanPham
    {
        public LoaiSanPham()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
