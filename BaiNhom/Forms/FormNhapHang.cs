using System;
using System.Linq;
using System.Windows.Forms;
using BaiNhom.Data;
using BaiNhom.Models;

namespace BaiNhom.Forms
{
    public partial class FormNhapHang : Form
    {
        public FormNhapHang()
        {
            InitializeComponent();
        }

        private void FormNhapHang_Load(object sender, EventArgs e)
        {
            LoadSanPham();
            LoadPhieuNhap();
        }

        private void LoadSanPham()
        {
            cboSanPham.DataSource = DataManager.Instance.DanhSachSanPham;
            cboSanPham.DisplayMember = "TenHang";
            cboSanPham.ValueMember = "MaHang";
        }

        private void LoadPhieuNhap()
        {
            dgvPhieuNhap.DataSource = null;
            dgvPhieuNhap.DataSource = DataManager.Instance.DanhSachPhieuNhap.OrderByDescending(x => x.NgayNhap).ToList();
            if (dgvPhieuNhap.Columns.Count > 0)
            {
                dgvPhieuNhap.Columns["MaPhieu"].HeaderText = "Mã phiếu";
                dgvPhieuNhap.Columns["MaHang"].HeaderText = "Mã hàng";
                dgvPhieuNhap.Columns["TenHang"].HeaderText = "Tên hàng";
                dgvPhieuNhap.Columns["SoLuongNhap"].HeaderText = "Số lượng nhập";
                dgvPhieuNhap.Columns["NgayNhap"].HeaderText = "Ngày nhập";
            }
        }

        private void cboSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedItem != null)
            {
                SanPham sp = (SanPham)cboSanPham.SelectedItem;
                lblTonKhoHienTai.Text = sp.SoLuongTon.ToString();
            }
        }

        private void btnNhapHang_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboSanPham.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int soLuongNhap = int.Parse(txtSoLuongNhap.Text);
                if (soLuongNhap <= 0)
                {
                    MessageBox.Show("Số lượng nhập phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SanPham sp = (SanPham)cboSanPham.SelectedItem;
                sp.SoLuongTon += soLuongNhap;

                PhieuNhap phieu = new PhieuNhap
                {
                    MaPhieu = DataManager.Instance.TaoMaPhieuNhapMoi(),
                    MaHang = sp.MaHang,
                    TenHang = sp.TenHang,
                    SoLuongNhap = soLuongNhap,
                    NgayNhap = DateTime.Now
                };

                DataManager.Instance.DanhSachPhieuNhap.Add(phieu);
                LoadPhieuNhap();
                lblTonKhoHienTai.Text = sp.SoLuongTon.ToString();
                txtSoLuongNhap.Clear();

                MessageBox.Show($"Nhập hàng thành công!\nTồn kho mới: {sp.SoLuongTon}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
