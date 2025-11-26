using System;
using System.Collections.Generic;

namespace BaiNhom.Models
{
    public class HoaDon
    {
        public string MaHD { get; set; }
        public DateTime NgayGiaoDich { get; set; }
        public KhachHang KhachHang { get; set; }
        public List<ChiTietHoaDon> ChiTiet { get; set; }
        public decimal TongTien { get; set; }
        public decimal KhuyenMai { get; set; }
        public decimal ThanhToan { get; set; }

        public HoaDon()
        {
            ChiTiet = new List<ChiTietHoaDon>();
            NgayGiaoDich = DateTime.Now;
        }
    }

    public class ChiTietHoaDon
    {
        public SanPham SanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien => SanPham.DonGia * SoLuong;
    }
}
