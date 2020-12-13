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
    public partial class frmGoiDichVu : Form
    {
        public frmGoiDichVu()
        {
            InitializeComponent();
            lblMaDV.Visible = false;
        }
        Data data = new Data();
        public string soPhong = null;
        HoaDon h;

        private void FrmGoiDichVu_Load(object sender, EventArgs e)
        {
            if (soPhong != null)
            {
                h = data.selectBill(soPhong);
                lblTenKhach.Text = "Khách hàng: " + data.getCustomerName(h.MaKH);
                cbbMaPhong.Visible = false;
            }
            else
            {
                lblSoPhong.Visible = false;
                cbbMaPhong.DataSource = data.getBusyRooms();
                cbbMaPhong.DisplayMember = "SoPhong";
                cbbMaPhong.ValueMember = "SoPhong";
                h = data.selectBill(cbbMaPhong.Text);
                lblTenKhach.Text = "Khách hàng: " + data.getCustomerName(h.MaKH);
            }
            loadListServices();
            loadDGVDichvu(h.MaHD);
            lblSoPhong.Text = "Phòng " + soPhong;
        }

        private void loadDGVDichvu(int MaHD)
        {
            dgvDichVu.RowHeadersVisible = false;
            dgvDichVu.DataSource = data.fillBillServices(MaHD);
            dgvDichVu.Columns["TenDV"].HeaderText = "Tên DV";
            dgvDichVu.Columns["SoLuong"].HeaderText = "Số Lượng";
            dgvDichVu.Columns["MaDV"].Visible = false;
            dgvDichVu.Columns["TenDV"].Width = 140;
            dgvDichVu.Columns["SoLuong"].Width = 140;
        }

        private void loadListServices()
        {
            dgvDSDichVu.RowHeadersVisible = false;
            dgvDSDichVu.DataSource = data.fillServiceTable();
            dgvDSDichVu.Columns["MaDV"].HeaderText = "Mã";
            dgvDSDichVu.Columns["MaDV"].Width = 40;
            dgvDSDichVu.Columns["TenDV"].HeaderText = "Tên DV";
            dgvDSDichVu.Columns["TenDV"].Width = 100;
            dgvDSDichVu.Columns["Gia"].HeaderText = "Giá";
            dgvDSDichVu.Columns["Gia"].Width = 70;
            dgvDSDichVu.Columns["DonVi"].HeaderText = "Đơn vị";
            dgvDSDichVu.Columns["DonVi"].Width = 60;
        }



        //private void btnXoa_Click(object sender, EventArgs e)
        //{
        //    if (lblMaDV2.Text == "")
        //    {
        //        MessageBox.Show("Vui lòng chọn dịch vụ!");
        //        return;
        //    }
        //    if (MessageBox.Show("Bạn có thực sự muốn xóa!", "Nhắc nhở", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //    {
        //        data.deleteServiceNumber(h.MaHD, Convert.ToInt32(lblMaDV2.Text));
        //        loadDGVDichvu(h.MaHD);
        //    }
        //}


        private void dgvDSDichVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int r = e.RowIndex;
                lblTenDV.Text = dgvDSDichVu.Rows[r].Cells["TenDV"].Value.ToString();
                lblDonVi.Text = dgvDSDichVu.Rows[r].Cells["DonVi"].Value.ToString();
                lblMaDV.Text = dgvDSDichVu.Rows[r].Cells["MaDV"].Value.ToString();
            }
            catch (Exception)
            {
            }
        }


        private void cbbMaPhong_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void btnThemDV_Click(object sender, EventArgs e)
        {
            if (lblTenDV.Text == "" || h == null)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ để thêm vào!");
                return;
            }
            if ((int)nudSoLuong.Value == 0)
            {
                MessageBox.Show("Bạn chưa chọn số lượng!");
                return;
            }
            int x = Convert.ToInt32(lblMaDV.Text);
            if (data.serviceExist(h.MaHD, x))
            {
                data.updateServiceNumber(h.MaHD, x, (int)nudSoLuong.Value);
            }
            else
            {
                data.addServiceNumber(h.MaHD, x, (int)nudSoLuong.Value);
            }
            loadDGVDichvu(h.MaHD);
        }

        //private void btnThayDoi_Click(object sender, EventArgs e)
        //{
        //    if (lblSoLuong.Text == "Số lượng")
        //    {
        //        MessageBox.Show("Bạn phải chọn dịch vụ đã!");
        //        return;
        //    }
        //    data.editServiceNumber(h.MaHD, Convert.ToInt32(lblMaDV2.Text), Convert.ToInt32(txtSoLuong.Text));
        //    loadDGVDichvu(h.MaHD);
        //}

        //private void dgvDichVu_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    try
        //    {
        //        int x = e.RowIndex;
        //        lblSoLuong.Text = dgvDSDichVu.Rows[x].Cells["TenDV"].Value.ToString();
        //        txtSoLuong.Text = dgvDSDichVu.Rows[x].Cells["SoLuong"].Value.ToString();
        //        lblMaDV2.Text = dgvDSDichVu.Rows[x].Cells["MaDV"].Value.ToString();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
    }
}
