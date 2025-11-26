using System;

namespace BaiNhom.Models
{
    public class SanPham
    {
        public string MaHang { get; set; }
        public string TenHang { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuongTon { get; set; }
        public DateTime HanSuDung { get; set; }

        public bool SapHetHan()
        {
            return (HanSuDung - DateTime.Now).TotalDays <= 30 && (HanSuDung - DateTime.Now).TotalDays >= 0;
        }

        public bool DaHetHan()
        {
            return HanSuDung < DateTime.Now;
        }
    }
}
