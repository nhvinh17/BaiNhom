using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BaiNhom.Data;
using BaiNhom.Models;

namespace BaiNhom.Forms
{
    public partial class FormBanHang : Form
    {
        private List<ChiTietHoaDon> gioHang;

        public FormBanHang()
        {
            InitializeComponent();
            gioHang = new List<ChiTietHoaDon>();
        }

        private void FormBanHang_Load(object sender, EventArgs e)
        {
            LoadKhachHang();
            LoadSanPham();
            LoadGioHang();
        }

        private void LoadKhachHang()
        {
            cboKhachHang.DataSource = DataManager.Instance.DanhSachKhachHang;
            cboKhachHang.DisplayMember = "TenKH";
            cboKhachHang.ValueMember = "MaKH";
        }

        private void LoadSanPham()
        {
            var sanPhamConHang = DataManager.Instance.DanhSachSanPham.Where(x => x.SoLuongTon > 0).ToList();
            cboSanPham.DataSource = sanPhamConHang;
            cboSanPham.DisplayMember = "TenHang";
            cboSanPham.ValueMember = "MaHang";
        }

        private void LoadGioHang()
        {
            dgvGioHang.DataSource = null;
            var gioHangDisplay = gioHang.Select(x => new
            {
                MaHang = x.SanPham.MaHang,
                TenHang = x.SanPham.TenHang,
                DonGia = x.SanPham.DonGia,
                SoLuong = x.SoLuong,
                ThanhTien = x.ThanhTien
            }).ToList();
            dgvGioHang.DataSource = gioHangDisplay;

            if (dgvGioHang.Columns.Count > 0)
            {
                dgvGioHang.Columns["MaHang"].HeaderText = "Mã hàng";
                dgvGioHang.Columns["TenHang"].HeaderText = "Tên hàng";
                dgvGioHang.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvGioHang.Columns["SoLuong"].HeaderText = "Số lượng";
                dgvGioHang.Columns["ThanhTien"].HeaderText = "Thành tiền";
                dgvGioHang.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                dgvGioHang.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            }

            TinhTien();
        }

        private void TinhTien()
        {
            decimal tongTien = gioHang.Sum(x => x.ThanhTien);
            decimal khuyenMai = 0;

            if (tongTien >= 800000 && tongTien <= 1000000)
            {
                khuyenMai = 80000;
            }

            decimal thanhToan = tongTien - khuyenMai;

            lblTongTien.Text = tongTien.ToString("N0") + " đ";
            lblKhuyenMai.Text = khuyenMai.ToString("N0") + " đ";
            lblThanhToan.Text = thanhToan.ToString("N0") + " đ";
        }

        private void btnThemVaoGio_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboSanPham.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SanPham sp = (SanPham)cboSanPham.SelectedItem;
                int soLuong = int.Parse(txtSoLuong.Text);

                if (soLuong <= 0)
                {
                    MessageBox.Show("Số lượng phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (soLuong > sp.SoLuongTon)
                {
                    MessageBox.Show($"Không đủ hàng! Tồn kho: {sp.SoLuongTon}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var existing = gioHang.FirstOrDefault(x => x.SanPham.MaHang == sp.MaHang);
                if (existing != null)
                {
                    existing.SoLuong += soLuong;
                }
                else
                {
                    gioHang.Add(new ChiTietHoaDon
                    {
                        SanPham = sp,
                        SoLuong = soLuong
                    });
                }

                LoadGioHang();
                txtSoLuong.Text = "1";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaKhoiGio_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow != null)
            {
                string maHang = dgvGioHang.CurrentRow.Cells["MaHang"].Value.ToString();
                var item = gioHang.FirstOrDefault(x => x.SanPham.MaHang == maHang);
                if (item != null)
                {
                    gioHang.Remove(item);
                    LoadGioHang();
                }
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboKhachHang.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (gioHang.Count == 0)
                {
                    MessageBox.Show("Giỏ hàng trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                KhachHang kh = (KhachHang)cboKhachHang.SelectedItem;
                decimal tongTien = gioHang.Sum(x => x.ThanhTien);
                decimal khuyenMai = 0;

                if (tongTien >= 800000 && tongTien <= 1000000)
                {
                    khuyenMai = 80000;
                }

                HoaDon hd = new HoaDon
                {
                    MaHD = DataManager.Instance.TaoMaHoaDonMoi(),
                    NgayGiaoDich = DateTime.Now,
                    KhachHang = kh,
                    ChiTiet = new List<ChiTietHoaDon>(gioHang),
                    TongTien = tongTien,
                    KhuyenMai = khuyenMai,
                    ThanhToan = tongTien - khuyenMai
                };

                foreach (var item in gioHang)
                {
                    var sp = DataManager.Instance.DanhSachSanPham.FirstOrDefault(x => x.MaHang == item.SanPham.MaHang);
                    if (sp != null)
                    {
                        sp.SoLuongTon -= item.SoLuong;
                    }
                }

                DataManager.Instance.DanhSachHoaDon.Add(hd);

                FormInHoaDon frmIn = new FormInHoaDon(hd);
                frmIn.ShowDialog();

                gioHang.Clear();
                LoadGioHang();
                LoadSanPham();

                MessageBox.Show("Thanh toán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn hủy đơn hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                gioHang.Clear();
                LoadGioHang();
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
