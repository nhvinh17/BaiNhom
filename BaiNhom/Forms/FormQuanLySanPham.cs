using System;
using System.Linq;
using System.Windows.Forms;
using BaiNhom.Data;
using BaiNhom.Models;

namespace BaiNhom.Forms
{
    public partial class FormQuanLySanPham : Form
    {
        public FormQuanLySanPham()
        {
            InitializeComponent();
        }

        private void FormQuanLySanPham_Load(object sender, EventArgs e)
        {
            LoadDanhSach();
            LamMoi();
        }

        private void LoadDanhSach()
        {
            dgvSanPham.DataSource = null;
            dgvSanPham.DataSource = DataManager.Instance.DanhSachSanPham;
            dgvSanPham.Columns["MaHang"].HeaderText = "Mã hàng";
            dgvSanPham.Columns["TenHang"].HeaderText = "Tên hàng";
            dgvSanPham.Columns["DonGia"].HeaderText = "Đơn giá";
            dgvSanPham.Columns["SoLuongTon"].HeaderText = "Số lượng tồn";
            dgvSanPham.Columns["HanSuDung"].HeaderText = "Hạn sử dụng";
            dgvSanPham.Columns["DonGia"].DefaultCellStyle.Format = "N0";
        }

        private void LamMoi()
        {
            txtMaHang.Text = DataManager.Instance.TaoMaSanPhamMoi();
            txtTenHang.Clear();
            txtDonGia.Clear();
            txtSoLuongTon.Clear();
            dtpHanSuDung.Value = DateTime.Now.AddDays(30);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTenHang.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SanPham sp = new SanPham
                {
                    MaHang = txtMaHang.Text,
                    TenHang = txtTenHang.Text,
                    DonGia = decimal.Parse(txtDonGia.Text),
                    SoLuongTon = int.Parse(txtSoLuongTon.Text),
                    HanSuDung = dtpHanSuDung.Value
                };

                DataManager.Instance.DanhSachSanPham.Add(sp);
                LoadDanhSach();
                LamMoi();
                MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                var sp = DataManager.Instance.DanhSachSanPham.FirstOrDefault(x => x.MaHang == txtMaHang.Text);
                if (sp != null)
                {
                    sp.TenHang = txtTenHang.Text;
                    sp.DonGia = decimal.Parse(txtDonGia.Text);
                    sp.SoLuongTon = int.Parse(txtSoLuongTon.Text);
                    sp.HanSuDung = dtpHanSuDung.Value;
                    LoadDanhSach();
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var sp = DataManager.Instance.DanhSachSanPham.FirstOrDefault(x => x.MaHang == txtMaHang.Text);
                if (sp != null)
                {
                    DataManager.Instance.DanhSachSanPham.Remove(sp);
                    LoadDanhSach();
                    LamMoi();
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnKiemTraTonKho_Click(object sender, EventArgs e)
        {
            var tonKhoThap = DataManager.Instance.DanhSachSanPham.Where(x => x.SoLuongTon < 20).ToList();
            dgvSanPham.DataSource = null;
            dgvSanPham.DataSource = tonKhoThap;
            MessageBox.Show($"Có {tonKhoThap.Count} sản phẩm tồn kho dưới 20!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnKiemTraHetHan_Click(object sender, EventArgs e)
        {
            var sapHetHan = DataManager.Instance.DanhSachSanPham.Where(x => x.SapHetHan()).ToList();
            dgvSanPham.DataSource = null;
            dgvSanPham.DataSource = sapHetHan;
            MessageBox.Show($"Có {sapHetHan.Count} sản phẩm sắp hết hạn (dưới 30 ngày)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];
                txtMaHang.Text = row.Cells["MaHang"].Value.ToString();
                txtTenHang.Text = row.Cells["TenHang"].Value.ToString();
                txtDonGia.Text = row.Cells["DonGia"].Value.ToString();
                txtSoLuongTon.Text = row.Cells["SoLuongTon"].Value.ToString();
                dtpHanSuDung.Value = Convert.ToDateTime(row.Cells["HanSuDung"].Value);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
