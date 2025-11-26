using System;
using System.Linq;
using System.Windows.Forms;
using BaiNhom.Data;

namespace BaiNhom.Forms
{
    public partial class FormBaoCao : Form
    {
        public FormBaoCao()
        {
            InitializeComponent();
        }

        private void FormBaoCao_Load(object sender, EventArgs e)
        {
            dtpTuNgay.Value = DateTime.Now.AddMonths(-1);
            dtpDenNgay.Value = DateTime.Now;
        }

        private void btnDoanhThu_Click(object sender, EventArgs e)
        {
            var hoaDons = DataManager.Instance.DanhSachHoaDon
                .Where(x => x.NgayGiaoDich >= dtpTuNgay.Value && x.NgayGiaoDich <= dtpDenNgay.Value)
                .OrderByDescending(x => x.NgayGiaoDich)
                .ToList();

            dgvBaoCao.DataSource = hoaDons.Select(x => new
            {
                MaHD = x.MaHD,
                NgayGiaoDich = x.NgayGiaoDich,
                KhachHang = x.KhachHang.TenKH,
                TongTien = x.TongTien,
                KhuyenMai = x.KhuyenMai,
                ThanhToan = x.ThanhToan
            }).ToList();

            if (dgvBaoCao.Columns.Count > 0)
            {
                dgvBaoCao.Columns["MaHD"].HeaderText = "Mã hóa đơn";
                dgvBaoCao.Columns["NgayGiaoDich"].HeaderText = "Ngày giao dịch";
                dgvBaoCao.Columns["KhachHang"].HeaderText = "Khách hàng";
                dgvBaoCao.Columns["TongTien"].HeaderText = "Tổng tiền";
                dgvBaoCao.Columns["KhuyenMai"].HeaderText = "Khuyến mãi";
                dgvBaoCao.Columns["ThanhToan"].HeaderText = "Thanh toán";
                dgvBaoCao.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                dgvBaoCao.Columns["KhuyenMai"].DefaultCellStyle.Format = "N0";
                dgvBaoCao.Columns["ThanhToan"].DefaultCellStyle.Format = "N0";
            }

            decimal tongDoanhThu = hoaDons.Sum(x => x.ThanhToan);
            lblThongKe.Text = $"Tổng doanh thu: {tongDoanhThu:N0} đ | Số hóa đơn: {hoaDons.Count}";
        }

        private void btnSanPhamBanChay_Click(object sender, EventArgs e)
        {
            var sanPhamBanChay = DataManager.Instance.DanhSachHoaDon
                .SelectMany(x => x.ChiTiet)
                .GroupBy(x => x.SanPham.MaHang)
                .Select(g => new
                {
                    MaHang = g.Key,
                    TenHang = g.First().SanPham.TenHang,
                    SoLuongBan = g.Sum(x => x.SoLuong),
                    DoanhThu = g.Sum(x => x.ThanhTien)
                })
                .OrderByDescending(x => x.SoLuongBan)
                .ToList();

            dgvBaoCao.DataSource = sanPhamBanChay;

            if (dgvBaoCao.Columns.Count > 0)
            {
                dgvBaoCao.Columns["MaHang"].HeaderText = "Mã hàng";
                dgvBaoCao.Columns["TenHang"].HeaderText = "Tên hàng";
                dgvBaoCao.Columns["SoLuongBan"].HeaderText = "Số lượng bán";
                dgvBaoCao.Columns["DoanhThu"].HeaderText = "Doanh thu";
                dgvBaoCao.Columns["DoanhThu"].DefaultCellStyle.Format = "N0";
            }

            lblThongKe.Text = $"Tổng số sản phẩm: {sanPhamBanChay.Count}";
        }

        private void btnTonKho_Click(object sender, EventArgs e)
        {
            var tonKho = DataManager.Instance.DanhSachSanPham
                .OrderBy(x => x.SoLuongTon)
                .Select(x => new
                {
                    MaHang = x.MaHang,
                    TenHang = x.TenHang,
                    SoLuongTon = x.SoLuongTon,
                    DonGia = x.DonGia,
                    GiaTriTon = x.SoLuongTon * x.DonGia,
                    HanSuDung = x.HanSuDung
                })
                .ToList();

            dgvBaoCao.DataSource = tonKho;

            if (dgvBaoCao.Columns.Count > 0)
            {
                dgvBaoCao.Columns["MaHang"].HeaderText = "Mã hàng";
                dgvBaoCao.Columns["TenHang"].HeaderText = "Tên hàng";
                dgvBaoCao.Columns["SoLuongTon"].HeaderText = "Số lượng tồn";
                dgvBaoCao.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvBaoCao.Columns["GiaTriTon"].HeaderText = "Giá trị tồn";
                dgvBaoCao.Columns["HanSuDung"].HeaderText = "Hạn sử dụng";
                dgvBaoCao.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                dgvBaoCao.Columns["GiaTriTon"].DefaultCellStyle.Format = "N0";
            }

            decimal tongGiaTriTon = tonKho.Sum(x => x.GiaTriTon);
            lblThongKe.Text = $"Tổng giá trị tồn kho: {tongGiaTriTon:N0} đ";
        }

        private void btnSapHetHan_Click(object sender, EventArgs e)
        {
            var sapHetHan = DataManager.Instance.DanhSachSanPham
                .Where(x => x.SapHetHan() || x.DaHetHan())
                .Select(x => new
                {
                    MaHang = x.MaHang,
                    TenHang = x.TenHang,
                    SoLuongTon = x.SoLuongTon,
                    HanSuDung = x.HanSuDung,
                    SoNgayConLai = (x.HanSuDung - DateTime.Now).Days,
                    TrangThai = x.DaHetHan() ? "Đã hết hạn" : "Sắp hết hạn"
                })
                .OrderBy(x => x.HanSuDung)
                .ToList();

            dgvBaoCao.DataSource = sapHetHan;

            if (dgvBaoCao.Columns.Count > 0)
            {
                dgvBaoCao.Columns["MaHang"].HeaderText = "Mã hàng";
                dgvBaoCao.Columns["TenHang"].HeaderText = "Tên hàng";
                dgvBaoCao.Columns["SoLuongTon"].HeaderText = "Số lượng tồn";
                dgvBaoCao.Columns["HanSuDung"].HeaderText = "Hạn sử dụng";
                dgvBaoCao.Columns["SoNgayConLai"].HeaderText = "Số ngày còn lại";
                dgvBaoCao.Columns["TrangThai"].HeaderText = "Trạng thái";
            }

            lblThongKe.Text = $"Số sản phẩm cần chú ý: {sapHetHan.Count}";
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
