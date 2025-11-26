using System;
using System.Windows.Forms;

namespace BaiNhom.Forms
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnQuanLySanPham_Click(object sender, EventArgs e)
        {
            FormQuanLySanPham frm = new FormQuanLySanPham();
            frm.ShowDialog();
        }

        private void btnQuanLyKhachHang_Click(object sender, EventArgs e)
        {
            FormQuanLyKhachHang frm = new FormQuanLyKhachHang();
            frm.ShowDialog();
        }

        private void btnBanHang_Click(object sender, EventArgs e)
        {
            FormBanHang frm = new FormBanHang();
            frm.ShowDialog();
        }

        private void btnNhapHang_Click(object sender, EventArgs e)
        {
            FormNhapHang frm = new FormNhapHang();
            frm.ShowDialog();
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            FormBaoCao frm = new FormBaoCao();
            frm.ShowDialog();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
