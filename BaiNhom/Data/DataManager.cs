using System;
using System.Collections.Generic;
using System.Linq;
using BaiNhom.Models;

namespace BaiNhom.Data
{
    public class DataManager
    {
        private static DataManager instance;
        public static DataManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new DataManager();
                return instance;
            }
        }

        public List<SanPham> DanhSachSanPham { get; set; }
        public List<KhachHang> DanhSachKhachHang { get; set; }
        public List<HoaDon> DanhSachHoaDon { get; set; }
        public List<PhieuNhap> DanhSachPhieuNhap { get; set; }

        private DataManager()
        {
            DanhSachSanPham = new List<SanPham>();
            DanhSachKhachHang = new List<KhachHang>();
            DanhSachHoaDon = new List<HoaDon>();
            DanhSachPhieuNhap = new List<PhieuNhap>();
            KhoiTaoDuLieuMau();
        }

        private void KhoiTaoDuLieuMau()
        {
            DanhSachSanPham.Add(new SanPham { MaHang = "SP001", TenHang = "Sữa tươi Vinamilk", DonGia = 35000, SoLuongTon = 50, HanSuDung = DateTime.Now.AddDays(45) });
            DanhSachSanPham.Add(new SanPham { MaHang = "SP002", TenHang = "Bánh mì sandwich", DonGia = 25000, SoLuongTon = 30, HanSuDung = DateTime.Now.AddDays(15) });
            DanhSachSanPham.Add(new SanPham { MaHang = "SP003", TenHang = "Nước ngọt Coca Cola", DonGia = 15000, SoLuongTon = 100, HanSuDung = DateTime.Now.AddDays(180) });

            DanhSachKhachHang.Add(new KhachHang { MaKH = "KH001", TenKH = "Nguyễn Thái Bình", SoDienThoai = "0901234567", DiaChi = "123 Lê Lợi, Q1, TP.HCM" });
            DanhSachKhachHang.Add(new KhachHang { MaKH = "KH002", TenKH = "Đức Anh", SoDienThoai = "0912345678", DiaChi = "456 Nguyễn Huệ, Q1, TP.HCM" });
        }

        public string TaoMaSanPhamMoi()
        {
            int max = 0;
            foreach (var sp in DanhSachSanPham)
            {
                if (sp.MaHang.StartsWith("SP"))
                {
                    int num;
                    if (int.TryParse(sp.MaHang.Substring(2), out num))
                    {
                        if (num > max) max = num;
                    }
                }
            }
            return "SP" + (max + 1).ToString("D3");
        }

        public string TaoMaKhachHangMoi()
        {
            int max = 0;
            foreach (var kh in DanhSachKhachHang)
            {
                if (kh.MaKH.StartsWith("KH"))
                {
                    int num;
                    if (int.TryParse(kh.MaKH.Substring(2), out num))
                    {
                        if (num > max) max = num;
                    }
                }
            }
            return "KH" + (max + 1).ToString("D3");
        }

        public string TaoMaHoaDonMoi()
        {
            return "HD" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        public string TaoMaPhieuNhapMoi()
        {
            return "PN" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
