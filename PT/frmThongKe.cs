using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PT
{
    public partial class frmThongKe : Form
    {
        public frmThongKe()
        {
            InitializeComponent();
        }
        Data data = new Data();
        private void frmThongKe_Load(object sender, EventArgs e)
        {
            //dgvHoaDon.SelectedIndex = 0;
            dgvHoaDon.DataSource = data.fillStatiscical();
            dgvHoaDon.Columns["MaHD"].HeaderText = "Mã HĐ";
            dgvHoaDon.Columns["MaHD"].Width = 50;
            dgvHoaDon.Columns["TenNV"].HeaderText = "Nhân Viên";
            dgvHoaDon.Columns["TenKH"].HeaderText = "Khách hàng";
            dgvHoaDon.Columns["SoPhong"].HeaderText = "Số phòng";
            dgvHoaDon.Columns["SoPhong"].Width = 50;
            dgvHoaDon.Columns["NgayVao"].HeaderText = "Ngày nhận phòng";
            dgvHoaDon.Columns["NgayVao"].Width = 150;
            dgvHoaDon.Columns["NgayRa"].HeaderText = "Ngày trả phòng";
            dgvHoaDon.Columns["NgayRa"].Width = 150;
            dgvHoaDon.Columns["ThoiGianThue"].HeaderText = "Thời gian thuê";
            dgvHoaDon.Columns["ThoiGianThue"].Width = 40;
            dgvHoaDon.Columns["DonVi"].HeaderText = "Đơn vị";
            dgvHoaDon.Columns["DonVi"].Width = 40;
            dgvHoaDon.Columns["TienPhong"].HeaderText = "Tiền phòng";
            dgvHoaDon.Columns["TienDichVu"].HeaderText = "Tiền dịch vụ";
            dgvHoaDon.Columns["TongTien"].HeaderText = "Tổng tiền";
        }
    }
}
