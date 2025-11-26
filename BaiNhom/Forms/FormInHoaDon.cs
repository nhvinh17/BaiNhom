using System;
using System.Text;
using System.Windows.Forms;
using BaiNhom.Models;

namespace BaiNhom.Forms
{
    public partial class FormInHoaDon : Form
    {
        private HoaDon hoaDon;

        public FormInHoaDon(HoaDon hd)
        {
            InitializeComponent();
            hoaDon = hd;
        }

        private void FormInHoaDon_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("========================================");
            sb.AppendLine("         HÓA ĐƠN BÁN HÀNG");
            sb.AppendLine("========================================");
            sb.AppendLine();
            sb.AppendLine($"Mã hóa đơn: {hoaDon.MaHD}");
            sb.AppendLine($"Ngày giờ: {hoaDon.NgayGiaoDich:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine();
            sb.AppendLine("--- THÔNG TIN KHÁCH HÀNG ---");
            sb.AppendLine($"Mã KH: {hoaDon.KhachHang.MaKH}");
            sb.AppendLine($"Tên KH: {hoaDon.KhachHang.TenKH}");
            sb.AppendLine($"SĐT: {hoaDon.KhachHang.SoDienThoai}");
            sb.AppendLine($"Địa chỉ: {hoaDon.KhachHang.DiaChi}");
            sb.AppendLine();
            sb.AppendLine("--- CHI TIẾT SẢN PHẨM ---");
            sb.AppendLine(string.Format("{0,-10} {1,-20} {2,10} {3,12}", "Mã hàng", "Tên hàng", "SL", "Thành tiền"));
            sb.AppendLine("--------------------------------------------------------");

            foreach (var item in hoaDon.ChiTiet)
            {
                sb.AppendLine(string.Format("{0,-10} {1,-20} {2,10} {3,12:N0}",
                    item.SanPham.MaHang,
                    item.SanPham.TenHang.Length > 20 ? item.SanPham.TenHang.Substring(0, 17) + "..." : item.SanPham.TenHang,
                    item.SoLuong,
                    item.ThanhTien));
            }

            sb.AppendLine("========================================");
            sb.AppendLine($"Tổng tiền:              {hoaDon.TongTien,15:N0} đ");
            sb.AppendLine($"Khuyến mãi:             {hoaDon.KhuyenMai,15:N0} đ");
            sb.AppendLine($"THANH TOÁN:             {hoaDon.ThanhToan,15:N0} đ");
            sb.AppendLine("========================================");
            sb.AppendLine();
            sb.AppendLine("     Cảm ơn quý khách! Hẹn gặp lại!");

            txtHoaDon.Text = sb.ToString();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
