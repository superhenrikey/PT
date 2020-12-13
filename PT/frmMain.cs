using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PT
{
    public partial class frmMain : Form
    {
        Data data = new Data();
        HoaDon h;
        public frmMain()
        {
            InitializeComponent();
        }

        //get width, height form
        private int formWidth()
        {
            return this.Width;
        }
        private int formHeight()
        {
            return this.Height;
        }
        //end


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void header_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            loadTab1();
        }
 /// <summary>
 ///=====================================Tab 1 ===========================================
 /// </summary>
        private void loadTab1()
        {

            
            tabControl1.Size = new System.Drawing.Size(formWidth() - 175, formHeight() - 50);


            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;

            tabControl2.Appearance = TabAppearance.FlatButtons;
            tabControl2.ItemSize = new Size(0, 1);
            tabControl2.SizeMode = TabSizeMode.Fixed;

            foreach (TabPage tab in tabControl1.TabPages)
            {
                tab.Text = "";
            }

            loadListView();
            countRooms();
            disableControlButtons();
        }
        private void btnAllRom_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }

        private void btnListPeople_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            if(slideMenu.Width == 45)
            {
                slideMenu.Visible = false;
                slideMenu.Width = 175;
                tabControl1.Location = new System.Drawing.Point(175, 47);
                //listViewRooms.Size = new System.Drawing.Size(formWidth() - 339, formHeight() - 50);
                tabControl1.Size = new System.Drawing.Size(formWidth()-175, formHeight()-50);
                Tran1.ShowSync(slideMenu);
                
            }
            else
            {
                slideMenu.Visible = false;
                slideMenu.Width = 45;
                tabControl1.Location = new System.Drawing.Point(45, 47);
                //listViewRooms.Size = new System.Drawing.Size(formWidth() - 339 + 175 - 45,formHeight()-50);
                tabControl1.Size = new System.Drawing.Size(formWidth() - 45, formHeight() - 50);
                Tran2.ShowSync(slideMenu);
                
            }
        }
        private void countRooms()
        {

            int[] status = data.getRoomsStatus();
            lblFree.Text = "Trống (" + status[0] + ")";
            lblWait.Text = "Đang dọn (" + status[1] + ")";
            lblBusy.Text = "Đang sử dụng (" + status[2] + ")";
        }
        private void loadListView()
        {
            listViewRooms.Items.Clear();
            foreach (DataRow dr in data.getRooms().Rows)
            {
                Phong p = new Phong();
                p.SoPhong = dr["SoPhong"].ToString();
                p.TinhTrang = Convert.ToInt32(dr["TinhTrang"]);
                ListViewItem item = new ListViewItem(p.SoPhong);
                item.ImageIndex = p.TinhTrang;
                switch (p.TinhTrang)
                {
                    case 0:
                        item.ForeColor = Color.Green;
                        break;
                    case 1:
                        item.ForeColor = Color.Orange;
                        break;
                    case 2:
                        item.ForeColor = Color.Red;
                        break;
                }
                listViewRooms.Items.Add(item);

            }
        }

        private void listViewRooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewRooms.SelectedItems.Count == 0)
            {
                disableControlButtons();
                lblSoPhong.Text = "";
                lblNgayRa.Text = "";
                btnXemGia.Visible = false;
                btnThemDichVu.Visible = false;
                btnThongTinKhach.Visible = false;
            }
            else
            {
                switch (listViewRooms.SelectedItems[0].ImageIndex)
                {
                    case 0:
                        btnDatPhong.Enabled = true;
                        btnDonXong.Enabled = false;
                        btnTraPhong.Enabled = false;
                        btnDonPhong.Enabled = true;
                        btnThemDichVu.Enabled = false;
                        btnThongTinKhach.Enabled = false;
                        break;
                    case 1:
                        btnDatPhong.Enabled = false;
                        btnDonXong.Enabled = true;
                        btnTraPhong.Enabled = false;
                        btnDonPhong.Enabled = false;
                        btnThemDichVu.Enabled = false;
                        btnThongTinKhach.Enabled = false;
                        break;
                    case 2:
                        btnDatPhong.Enabled = false;
                        btnDonXong.Enabled = false;
                        btnTraPhong.Enabled = true;
                        btnDonPhong.Enabled = false;
                        btnThemDichVu.Enabled = true;
                        btnThongTinKhach.Enabled = true;
                        break;
                }
                h = data.selectBill(listViewRooms.SelectedItems[0].Text);
                if (h != null)
                {
                    lblSoPhong.Text = "Phòng " + h.SoPhong;
                    DateTime d = DateTime.Parse(h.NgayRa);
                    lblNgayRa.Text = "Ngày trả phòng: " + d.ToString("dd-MM-yyyy HH:mm");
                }
                else
                {
                    lblSoPhong.Text = "Phòng " + listViewRooms.SelectedItems[0].Text;
                    lblNgayRa.Text = "";
                }
                btnXemGia.Visible = true;
                btnThemDichVu.Visible = true;
                btnThongTinKhach.Visible = true;
            }
        }
        private void disableControlButtons()
        {
            btnDatPhong.Enabled = false;
            btnDonXong.Enabled = false;
            btnTraPhong.Enabled = false;
            btnDonPhong.Enabled = false;
            btnThemDichVu.Enabled = false;
            btnXemGia.Enabled = false;
            btnThongTinKhach.Enabled = false;
        }

        private void btnDonPhong_Click(object sender, EventArgs e)
        {
            try
            {
                data.clearRoom(listViewRooms.SelectedItems[0].Text);
                loadListView();
                countRooms();
                disableControlButtons();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Bạn chưa chọn mục nào!");
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra, xin kiểm tra lại!");
            }
        }

        private void btnDonXong_Click(object sender, EventArgs e)
        {
            try
            {
                data.clearDone(listViewRooms.SelectedItems[0].Text);
                loadListView();
                countRooms();
                disableControlButtons();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Bạn chưa chọn mục nào!");
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra, xin kiểm tra lại!");
            }
        }

        /// <summary>
        /// ======================================= TAB 2 ================================
        /// </summary>

        private void btnInfo_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
            LoadKieuPhong();
            cbbLoaiPhong_SelectedIndexChanged(null,null);
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            DanhSach();
            tabControl1.SelectedTab = tabPage4;
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage5;
            LoadTapCaNhan();
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage6;
        }


        ///////////////////////
        //          DS khách hàng
        ///////////////////////

        private void DanhSach()
        {
            for (int i = 0; i < dataGridViewX1.RowCount - 1; i += 2)
            {
                dataGridViewX1.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
            }
            dataGridViewX1.DataSource = data.getListCustomer();
            dataGridViewX1.RowHeadersVisible = false;
            dataGridViewX1.Columns["MaKH"].HeaderText = "Mã KH";
            dataGridViewX1.Columns["HoTen"].HeaderText = "Họ tên";
            dataGridViewX1.Columns["CMT"].HeaderText = "Chứng minh thư";
            dataGridViewX1.Columns["DiaChi"].HeaderText = "Địa chỉ";
            dataGridViewX1.Columns["SDT"].HeaderText = "Số điện thoại";

            dataGridViewX1.Columns["MaKH"].Width = 50;
            dataGridViewX1.Columns["HoTen"].Width = 160;
            dataGridViewX1.Columns["CMT"].Width = 100;
            dataGridViewX1.Columns["DiaChi"].Width = 200;
            dataGridViewX1.Columns["SDT"].Width = 100;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            dataGridViewX1.DataSource = data.seachPeople(txtHoTen.text);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            lblBG.Location = new System.Drawing.Point(3, 18);
            setDis();
            pictureBox9.BackColor = System.Drawing.Color.DarkGray;
            tabControl2.SelectedTab = tabPage8;
            label48.BackColor = System.Drawing.Color.DarkGray;
            label48.ForeColor = System.Drawing.Color.White;
        }
        private void pictureBox10_Click(object sender, EventArgs e)
        {
            lblBG.Location = new System.Drawing.Point(3, 98);
            setDis();
            pictureBox10.BackColor = System.Drawing.Color.DarkGray;
            tabControl2.SelectedTab = tabPage9;
            label21.BackColor = System.Drawing.Color.DarkGray;
            label21.ForeColor = System.Drawing.Color.White;

            loadDV();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            lblBG.Location = new System.Drawing.Point(3, 178);
            setDis();
            pictureBox11.BackColor = System.Drawing.Color.DarkGray;
            tabControl2.SelectedTab = tabPage10;
            label19.BackColor = System.Drawing.Color.DarkGray;
            label19.ForeColor = System.Drawing.Color.White;

            loadNV();
            if (AccountInfo.level == 1)
            {
                cbbChucVu.DataSource = data.fillLevelCBB();
                cbbChucVu.DisplayMember = "MoTa";
                cbbChucVu.ValueMember = "Cap";
            }
            if (AccountInfo.level == 2)
            {
                cbbChucVu.DataSource = data.fillLevel2CBB();
                cbbChucVu.DisplayMember = "MoTa";
                cbbChucVu.ValueMember = "Cap";
            }
        }
        private void setDis()
        {
            pictureBox9.BackColor = System.Drawing.Color.Transparent;
            pictureBox10.BackColor = System.Drawing.Color.Transparent;
            pictureBox11.BackColor = System.Drawing.Color.Transparent;
            pictureBox12.BackColor = System.Drawing.Color.Transparent;

            //lblBG.BackColor = System.Drawing.Color.Transparent;
            label48.BackColor = System.Drawing.Color.Transparent;
            label19.BackColor = System.Drawing.Color.Transparent;
            label20.BackColor = System.Drawing.Color.Transparent;
            label21.BackColor = System.Drawing.Color.Transparent;

            //lblBG.ForeColor = System.Drawing.Color.Black;
            label48.ForeColor = System.Drawing.Color.Black;
            label19.ForeColor = System.Drawing.Color.Black;
            label20.ForeColor = System.Drawing.Color.Black;
            label21.ForeColor = System.Drawing.Color.Black;
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            lblBG.Location = new System.Drawing.Point(3, 258);
            setDis();
            pictureBox12.BackColor = System.Drawing.Color.DarkGray;
            tabControl2.SelectedTab = tabPage11;
            label20.BackColor = System.Drawing.Color.DarkGray;
            label20.ForeColor = System.Drawing.Color.White;

            loadQLP();
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        ///////////////////////
        //                  Giá Phòng
        ///////////////////////
        private void LoadKieuPhong()
        {
            cbbLoaiPhong.DataSource = data.getRoomType();
            cbbLoaiPhong.DisplayMember = "MoTa";
            cbbLoaiPhong.ValueMember = "LoaiPhong";
            cbbLoaiPhong.SelectedValue = "1";
        }
        GiaPhong g;
        private void cbbLoaiPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbLoaiPhong.SelectedIndex == 0)
                g = data.getPriceType(1);
            else
                g = data.getPriceType(2);
            if (g != null)
            {
                txtGiaGioDau.Text = g.GiaGioDau.ToString();
                txtGia2GioDau.Text = g.Gia2GioDau.ToString();
                txtGia3GioDau.Text = g.Gia3GioDau.ToString();
                txtGiaMoiGioTiepTheo.Text = g.GiaCacGioConLai.ToString();
                txtGiaTheoNgay.Text = g.GiaTheoNgay.ToString();
                txtGiaQuaDem.Text = g.GiaQuaDem.ToString();
                txtPhuThu.Text = g.PhuThu.ToString();
            }
        }
        private void btnCapNhatGia_Click(object sender, EventArgs e)
        {
            if (btnThayDoi.Text == "Thay đổi giá")
            {
                enableTxt(true);
                btnThayDoi.Text = "Hoàn thành";
                ActiveControl = txtGiaGioDau;
            }
            else
            {
                GiaPhong gia = new GiaPhong()
                {
                    
                    LoaiPhong = cbbLoaiPhong.SelectedIndex + 1,
                    GiaGioDau = Convert.ToSingle(txtGiaGioDau.Text),
                    Gia2GioDau = Convert.ToSingle(txtGia2GioDau.Text),
                    Gia3GioDau = Convert.ToSingle(txtGia3GioDau.Text),
                    GiaCacGioConLai = Convert.ToSingle(txtGiaMoiGioTiepTheo.Text),
                    GiaQuaDem = Convert.ToSingle(txtGiaQuaDem.Text),
                    GiaTheoNgay = Convert.ToSingle(txtGiaTheoNgay.Text),
                    PhuThu = Convert.ToSingle(txtPhuThu.Text)
                };
                data.updatePrice(gia);
                btnThayDoi.Text = "Thay đổi giá";
                enableTxt(false);
                MessageBox.Show("Cập nhật giá thành công!");
            }
        }
        private void enableTxt(bool b)
        {
            txtGiaGioDau.Enabled = b;
            txtGia2GioDau.Enabled = b;
            txtGia3GioDau.Enabled = b;
            txtGiaMoiGioTiepTheo.Enabled = b;
            txtGiaQuaDem.Enabled = b;
            txtGiaTheoNgay.Enabled = b;
            txtPhuThu.Enabled = b;
        }

        private void btnXemGia_Click(object sender, EventArgs e)
        {
            frmGiaPhong f = new frmGiaPhong();
            f.SoPhong = listViewRooms.SelectedItems[0].Text;
            f.ShowDialog();
        }

        private void btnDatPhong_Click(object sender, EventArgs e)
        {
            try
            {
                frmDatPhong f = new frmDatPhong();
                f.SoPhong = listViewRooms.SelectedItems[0].Text;
                f.ShowDialog();
                loadListView();
                countRooms();
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra. Kiểm tra lại !", "Error");
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            frmDatPhong f = new frmDatPhong();
            f.ShowDialog();
            loadListView();
            countRooms();
        }



        ///////////////////////
        //          Thông tin khách hàng
        ///////////////////////

        private void btnThongTinKhach_Click(object sender, EventArgs e)
        {
            KhachHang k = new KhachHang();
            k = data.getThongTinKH(listViewRooms.SelectedItems[0].Text);
            MessageBox.Show("Họ tên :  "+k.HoTen+"\nCMT :  " + k.CMT+"\nĐịa chỉ :  "+k.DiaChi+"\nSố ĐT :  "+k.SDT,"Thông tin KH", 0,MessageBoxIcon.Question);
        }

        private void btnTraPhong_Click(object sender, EventArgs e)
        {
            try
            {
                frmTraPhong f = new frmTraPhong();
                f.SoPhong = listViewRooms.SelectedItems[0].Text;
                f.ShowDialog();
                loadListView();
                countRooms();
                disableControlButtons();
            }
            catch (Exception)
            {
                MessageBox.Show("Bạn chưa chọn phòng !","Error");
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            frmTraPhong f = new frmTraPhong();
            f.ShowDialog();
            loadListView();
            countRooms();
            disableControlButtons();
        }

        private void btnThemDichVu_Click(object sender, EventArgs e)
        {
            try
            {
                frmGoiDichVu f = new frmGoiDichVu();
                f.soPhong = listViewRooms.SelectedItems[0].Text;
                f.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Đã có lỗi, vui lòng xem lại !", "Error");
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            try
            {
                frmGoiDichVu f = new frmGoiDichVu();
                f.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Đã có lỗi, vui lòng xem lại !", "Error");
            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            frmThongKe f = new frmThongKe();
            f.ShowDialog();
        }

        ///////////////////////
        //          TAB Cá Nhân
        ///////////////////////
        private void LoadTapCaNhan()
        {
            NhanVien n = data.getEmployeeInfo(AccountInfo.employeeID);
            if (n == null) return;
            txtHTen.Text = n.HoTen;
            txtDiaChi.Text = n.DiaChi;
            txtNgaySinh.Text = n.NgaySinh;
            txtSDT.Text = n.SDT;
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if(btnCapNhat.Text.Equals("Cập nhật thông tin"))
            {
                btnCapNhat.Text = "Lưu";
                enTextBox(true);
            }
            else
            {
                enTextBox(false);
                NhanVien nv = new NhanVien()
                {
                    MaNV = AccountInfo.employeeID,
                    HoTen = txtHTen.Text,
                    DiaChi = txtDiaChi.Text,
                    NgaySinh = txtNgaySinh.Text,
                    SDT = txtSDT.Text
                };
                data.updateInfo(nv);
                btnCapNhat.Text = "Cập nhật thông tin";
                MessageBox.Show("Cập nhật thành công!");
            }
        }
        public void enTextBox(bool b)
        {
            txtHTen.Enabled = b;
            txtNgaySinh.Enabled = b;
            txtDiaChi.Enabled = b;
            txtSDT.Enabled = b;
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            frmDoiMatKhau f = new frmDoiMatKhau();
            f.ShowDialog();
        }

        ///////////////////////
        //          TAB Dịch Vụ
        ///////////////////////

        private void loadDV()
        {
            dgvDichVu.DataSource = data.fillServiceTable();
            dgvDichVu.Columns["MaDV"].HeaderText = "Mã DV";
            dgvDichVu.Columns["MaDV"].Width = 50;
            dgvDichVu.Columns["TenDV"].HeaderText = "Tên DV";
            dgvDichVu.Columns["Gia"].HeaderText = "Giá";
            dgvDichVu.Columns["DonVi"].HeaderText = "Đơn vị";
            enableTextBox(false);
            visibleOKCancelButtons(false);
            enableControlButtons(true);
        }

        private void enableTextBox(bool b)
        {
            txtGia.Enabled = b;
            txtDonVi.Enabled = b;
            txtMDV.Enabled = b;
            txtTenDV.Enabled = b;
        }

        private void clearTextBox()
        {
            txtGia.Clear();
            txtDonVi.Clear();
            txtMDV.Clear();
            txtTenDV.Clear();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtMDV.Text == "")
            {
                DichVu d = new DichVu()
                {
                    TenDV = txtTenDV.Text,
                    Gia = Convert.ToSingle(txtGia.Text),
                    DonVi = txtDonVi.Text
                };
                data.addService(d);
            }
            else
            {
                DichVu d = new DichVu()
                {
                    MaDV = Convert.ToInt32(txtMDV.Text),
                    TenDV = txtTenDV.Text,
                    Gia = Convert.ToSingle(txtGia.Text),
                    DonVi = txtDonVi.Text
                };
                data.editService(d);
            }
            loadDV();
        }

        private void visibleOKCancelButtons(bool b)
        {
            btnOK.Visible = b;
            btnHuy.Visible = b;
        }

        private void enableControlButtons(bool b)
        {
            btnThem.Enabled = b;
            btnSua.Enabled = b;
            btnXoa.Enabled = b;
            dgvDichVu.Enabled = b;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            enableTextBox(true);
            txtMDV.Enabled = false;
            clearTextBox();
            ActiveControl = txtTenDV;
            visibleOKCancelButtons(true);
            enableControlButtons(false);
        }



        private void dgvService_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = e.RowIndex;
            txtMDV.Text = dgvDichVu.Rows[r].Cells["MaDV"].Value.ToString();
            txtTenDV.Text = dgvDichVu.Rows[r].Cells["TenDV"].Value.ToString();
            txtGia.Text = dgvDichVu.Rows[r].Cells["Gia"].Value.ToString();
            txtDonVi.Text = dgvDichVu.Rows[r].Cells["DonVi"].Value.ToString();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            loadDV();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMDV.Text != "")
            {
                enableTextBox(true);
                ActiveControl = txtTenDV;
                txtMDV.Enabled = false;
                enableControlButtons(false);
                visibleOKCancelButtons(true);
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn mục nào!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMDV.Text == "")
            {
                MessageBox.Show("Bạn vui lòng chọn dịch vụ cần xóa!");
                return;
            }
            if (MessageBox.Show("Bạn có thật sự muốn xóa?", "Lời nhắc", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int m = Convert.ToInt32(txtMDV.Text);
                data.deleteSevice(m);
                loadDV();
            }
        }

        ///////////////////////
        //              TAB Nhân viên
        ///////////////////////


        private void loadNV()
        {
            enableInput(false);
            dgvNhanVien.DataSource = data.fillEmployees();
            dgvNhanVien.Columns["MaNV"].HeaderText = "Mã";
            dgvNhanVien.Columns["MaNV"].Width = 50;
            dgvNhanVien.Columns["HoTen"].HeaderText = "Họ tên";
            dgvNhanVien.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
            dgvNhanVien.Columns["DiaChi"].HeaderText = "Địa chỉ";
            dgvNhanVien.Columns["SDT"].HeaderText = "Số ĐT";
            dgvNhanVien.Columns["TaiKhoan"].HeaderText = "Tài khoản";
            dgvNhanVien.Columns["MoTa"].HeaderText = "Chức vụ";
            clearNV();
        }

        private void enableInput(bool b)
        {
            txtTab10HoTen.Enabled = b;
            txtTab10TaiKhoan.Enabled = b;
            txtTab10MatKhau.Enabled = b;
            txtTab10DiaChi.Enabled = b;
            dtiNgaySinh.Enabled = b;
            txtTab10SDT.Enabled = b;
            cbbChucVu.Enabled = b;
            btnTab10OK.Visible = b;
            btnTab10Huy.Visible = b;
            btnTab10Them.Enabled = !b;
            btnTab10Sua.Enabled = !b;
            btnTab10Xoa.Enabled = !b;
            dgvNhanVien.Enabled = !b;
            btnHide.Visible = b;
        }

        private void clearNV()
        {
            txtTab10Ma.Clear();
            txtTab10HoTen.Clear();
            txtTab10TaiKhoan.Clear();
            //dtiNgaySinh.ResetValue();
            dtiNgaySinh.ResetText();
            txtTab10DiaChi.Clear();
            txtTab10SDT.Clear();
            txtTab10MatKhau.Clear();
            //cbbChucVu.SelectedIndex = 0;
        }

        private void btnTab10OK_Click(object sender, EventArgs e)
        {

            if (txtTab10Ma.Text == "")
            {
                try
                {
                    NhanVien n = new NhanVien()
                    {
                        HoTen = txtTab10HoTen.Text,
                        NgaySinh = dtiNgaySinh.Value.ToString("dd-MM-yyyy"),
                        DiaChi = txtTab10DiaChi.Text,
                        SDT = txtTab10SDT.Text,
                        TaiKhoan = txtTab10TaiKhoan.Text,
                        MatKhau = txtTab10MatKhau.Text,
                        QuyenTruyCap = Convert.ToInt32(cbbChucVu.SelectedValue)
                    };
                    data.addEmployee(n);
                }
                catch (Exception)
                {
                    MessageBox.Show("Có lỗi, không thể thêm, thử lại sau !", "Error");
                }
            }
            else
            {
                try
                {
                    if (txtTab10MatKhau.Text == "")
                    {
                        MessageBox.Show("vui lòng nhập mật khẩu!");
                        ActiveControl = txtTab10MatKhau;
                        return;
                    }
                    NhanVien n = new NhanVien()
                    {
                        MaNV = Convert.ToInt32(txtTab10Ma.Text),
                        HoTen = txtTab10HoTen.Text,
                        NgaySinh = dtiNgaySinh.Value.ToString("dd-MM-yyyy"),
                        DiaChi = txtTab10DiaChi.Text,
                        SDT = txtTab10SDT.Text,
                        TaiKhoan = txtTab10TaiKhoan.Text,
                        MatKhau = txtTab10MatKhau.Text,
                        QuyenTruyCap = Convert.ToInt32(cbbChucVu.SelectedValue)
                    };
                    data.editEmployee(n);
                }
                catch (Exception)
                {
                    MessageBox.Show("Có lỗi, không thể sửa, thử lại sau !", "Error");
                }
            }
            loadNV();
        }


        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int r = e.RowIndex;
                txtTab10Ma.Text = dgvNhanVien.Rows[r].Cells["MaNV"].Value.ToString();
                txtTab10HoTen.Text = dgvNhanVien.Rows[r].Cells["HoTen"].Value.ToString();
                txtTab10SDT.Text = dgvNhanVien.Rows[r].Cells["SDT"].Value.ToString();
                txtTab10TaiKhoan.Text = dgvNhanVien.Rows[r].Cells["TaiKhoan"].Value.ToString();
                dtiNgaySinh.Text = dgvNhanVien.Rows[r].Cells["NgaySinh"].Value.ToString();
                txtTab10DiaChi.Text = dgvNhanVien.Rows[r].Cells["DiaChi"].Value.ToString();
                txtTab10MatKhau.Text = "***************";
                cbbChucVu.SelectedIndex = cbbChucVu.FindStringExact(dgvNhanVien.Rows[r].Cells["MoTa"].Value.ToString());
            }
            catch (Exception)
            {
            }
        }

        private void btnTab10Them_Click(object sender, EventArgs e)
        {
            enableInput(true);
            clearNV();
            ActiveControl = txtTab10HoTen;
        }

        private void btnTab10Sua_Click(object sender, EventArgs e)
        {
            if (txtTab10Ma.Text == "")
            {
                MessageBox.Show("Bạn hãy chọn 1 nhân viên để sửa!");
                return;
            }
            int r = dgvNhanVien.SelectedCells[0].RowIndex;
            string x = dgvNhanVien.Rows[r].Cells["MoTa"].Value.ToString();
            if (AccountInfo.level == 2 && (x == "Quản trị viên" || x == "Quản lý"))
            {
                MessageBox.Show("Bạn không thê sửa người này vì người này có cấp độ ngang hoặc cao hơn bạn!");
                return;
            }
            enableInput(true);
            txtTab10MatKhau.Clear();
            ActiveControl = txtTab10HoTen;
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            if (btnHide.Text == "Hiện")
            {
                btnHide.Text = "Ẩn";
                txtTab10MatKhau.PasswordChar = (char)0;
                txtTab10MatKhau.Text = txtTab10MatKhau.Text;
            }
            else
            {
                btnHide.Text = "Hiện";
                txtTab10MatKhau.PasswordChar = '*';

            }
        }

        private void btnTab10Huy_Click(object sender, EventArgs e)
        {
            loadNV();
        }

        private void btnTab10Xoa_Click(object sender, EventArgs e)
        {
            if (txtTab10Ma.Text == "")
            {
                MessageBox.Show("Bạn hãy chọn một nhân viên để xóa!");
                return;
            }
            int r = dgvNhanVien.SelectedCells[0].RowIndex;
            string x = dgvNhanVien.Rows[r].Cells["MoTa"].Value.ToString();
            if (AccountInfo.level == 2 && (x == "Quản trị viên" || x == "Quản lý"))
            {
                MessageBox.Show("Bạn không thê xóa người này vì người này có cấp độ ngang hoặc cao hơn bạn!");
                return;
            }
            try
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Nhắc nhở", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    data.deleteEmployee(Convert.ToInt32(txtTab10Ma.Text));
                MessageBox.Show("Đã xóa!", "Done !");
                loadNV();
            }
            catch(Exception)
            {
                MessageBox.Show("Có lỗi, thử lại sau !", "Error");
            }
        }
        ///////////
        //          Quản lí phòng
        ///////////
        bool x = true;
        string soPhong = "";

        private void loadQLP()
        {
            dgvPhong.DataSource = data.fillRooms();
            dgvPhong.Columns["SoPhong"].HeaderText = "Tên phòng";
            dgvPhong.Columns["MoTa"].HeaderText = "Loại phòng";
            dgvPhong.Columns["LoaiPhong"].Visible = false;
            dgvPhong.Columns["TinhTrang"].Visible = false;
            cbbTab11LoaiPhong.DataSource = data.getRoomType();
            cbbTab11LoaiPhong.DisplayMember = "MoTa";
            cbbTab11LoaiPhong.ValueMember = "LoaiPhong";
            enable(false);
            visibleButtons(false);
        }

        private void btnTab11OK_Click(object sender, EventArgs e)
        {
            try
            {
                Phong p = new Phong()
                {
                    SoPhong = txtTenPhong.Text,
                    LoaiPhong = Convert.ToInt32(cbbTab11LoaiPhong.SelectedValue),
                    TinhTrang = 0
                };
                if (x)
                    data.addRoom(p);
                else
                {
                    data.editRoom(soPhong, p);
                }
                loadQLP();
            }
            catch (Exception)
            {
                MessageBox.Show("Trùng tên phòng!");
            }
        }

        private void enable(bool b)
        {
            txtTenPhong.Enabled = b;
            cbbTab11LoaiPhong.Enabled = b;
        }

        private void visibleButtons(bool b)
        {
            btnTab11Them.Enabled = !b;
            btnTab11Sua.Enabled = !b;
            btnTab11Xoa.Enabled = !b;
            btnTab11OK.Visible = b;
            btnTab11Huy.Visible = b;
            dgvPhong.Enabled = !b;
        }

        private void btnTab11Them_Click(object sender, EventArgs e)
        {
            enable(true);
            txtTenPhong.Clear();
            cbbTab11LoaiPhong.SelectedIndex = 0;
            visibleButtons(true);
            x = true;
            ActiveControl = txtTenPhong;
        }

        private void btnTab11Sua_Click(object sender, EventArgs e)
        {
            if (txtTenPhong.Text == "")
            {
                MessageBox.Show("Bạn phải chọn phòng đã");
                return;
            }
            soPhong = txtTenPhong.Text;
            enable(true);
            visibleButtons(true);
            x = false;
            ActiveControl = txtTenPhong;
        }

        private void btnTab11Huy_Click(object sender, EventArgs e)
        {
            enable(false);
            visibleButtons(false);
        }

        private void dgvPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int r = e.RowIndex;
                txtTenPhong.Text = dgvPhong.Rows[r].Cells["SoPhong"].Value.ToString();
                cbbTab11LoaiPhong.SelectedValue = dgvPhong.Rows[r].Cells["LoaiPhong"].Value.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void btnTab11Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTenPhong.Text == "")
                {
                    MessageBox.Show("Bạn phải chọn phòng cần xóa!");
                    return;
                }
                if (Convert.ToInt32(dgvPhong.Rows[dgvPhong.SelectedCells[0].RowIndex].Cells["TinhTrang"].Value) == 2)
                {
                    MessageBox.Show("Phòng này đang có người ở, vui lòng thanh toán trước!");
                    return;
                }
                if (MessageBox.Show("Bạn có thật sự muốn xóa?", "Nhắc nhở", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    data.deleteRoom(txtTenPhong.Text);
                    loadQLP();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi, thử lại sau!", "Error");
            }
        }
    }

}
