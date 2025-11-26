using System;
using System.Linq;
using System.Windows.Forms;
using BaiNhom.Data;
using BaiNhom.Models;

namespace BaiNhom.Forms
{
    public partial class FormQuanLyKhachHang : Form
    {
        public FormQuanLyKhachHang()
        {
            InitializeComponent();
        }

        private void FormQuanLyKhachHang_Load(object sender, EventArgs e)
        {
            LoadDanhSach();
            LamMoi();
        }

        private void LoadDanhSach()
        {
            dgvKhachHang.DataSource = null;
            dgvKhachHang.DataSource = DataManager.Instance.DanhSachKhachHang;
            dgvKhachHang.Columns["MaKH"].HeaderText = "Mã KH";
            dgvKhachHang.Columns["TenKH"].HeaderText = "Tên khách hàng";
            dgvKhachHang.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
            dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa chỉ";
        }

        private void LamMoi()
        {
            txtMaKH.Text = DataManager.Instance.TaoMaKhachHangMoi();
            txtTenKH.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();
            txtTimKiem.Clear();
            LoadDanhSach();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTenKH.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                KhachHang kh = new KhachHang
                {
                    MaKH = txtMaKH.Text,
                    TenKH = txtTenKH.Text,
                    SoDienThoai = txtSoDienThoai.Text,
                    DiaChi = txtDiaChi.Text
                };

                DataManager.Instance.DanhSachKhachHang.Add(kh);
                LoadDanhSach();
                LamMoi();
                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                var kh = DataManager.Instance.DanhSachKhachHang.FirstOrDefault(x => x.MaKH == txtMaKH.Text);
                if (kh != null)
                {
                    kh.TenKH = txtTenKH.Text;
                    kh.SoDienThoai = txtSoDienThoai.Text;
                    kh.DiaChi = txtDiaChi.Text;
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
                var kh = DataManager.Instance.DanhSachKhachHang.FirstOrDefault(x => x.MaKH == txtMaKH.Text);
                if (kh != null)
                {
                    DataManager.Instance.DanhSachKhachHang.Remove(kh);
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

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.ToLower();
            var result = DataManager.Instance.DanhSachKhachHang
                .Where(x => x.TenKH.ToLower().Contains(keyword) || x.SoDienThoai.Contains(keyword))
                .ToList();
            dgvKhachHang.DataSource = null;
            dgvKhachHang.DataSource = result;
            MessageBox.Show($"Tìm thấy {result.Count} khách hàng!", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];
                txtMaKH.Text = row.Cells["MaKH"].Value.ToString();
                txtTenKH.Text = row.Cells["TenKH"].Value.ToString();
                txtSoDienThoai.Text = row.Cells["SoDienThoai"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
