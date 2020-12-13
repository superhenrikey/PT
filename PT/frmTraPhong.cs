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
    public partial class frmTraPhong : Form
    {
        public frmTraPhong()
        {
            InitializeComponent();
        }
        public string SoPhong = null;
        public double soNgay = 0;
        public double SoGio = 0;
        Data data = new Data();
        HoaDon h;
        ChiTietHoaDon c;
        private void FrmTraPhong_Load(object sender, EventArgs e)
        {

            if (SoPhong != null)
            {
                formLoad(SoPhong);
                cbbSoPhong.Visible = false;
            }
            else
            {
                cbbSoPhong.DataSource = data.getBusyRooms();
                cbbSoPhong.DisplayMember = "SoPhong";
                cbbSoPhong.ValueMember = "SoPhong";
                formLoad(cbbSoPhong.SelectedValue.ToString());
            }
        }

        private void formLoad(string SoPhong)
        {
            c = new ChiTietHoaDon();
            lblSoPhong.Text = SoPhong;
            h = data.selectBill(SoPhong);
            c.MaHD = h.MaHD;
            lblSoHoaDon.Text = h.MaHD.ToString();
            lblKhachHang.Text = data.getCustomerName(h.MaKH);
            lblNhanVien.Text = data.getEmployeeName(h.MaNV);
            DateTime date = DateTime.Parse(h.NgayVao);
            lblNgayVao.Text = date.ToString("dd-MM-yyyy");
            lblGioVao.Text = date.ToString("HH:mm");
            DateTime dateOut = DateTime.Parse(h.NgayRa);
            lblNgayRa.Text = dateOut.ToString("dd-MM-yyyy");
            lblGioRa.Text = dateOut.ToString("HH:mm");
            lblNgayHienTai.Text = DateTime.Today.ToString("dd-MM-yyyy");
            lblGioHienTai.Text = DateTime.Now.ToString("HH:mm");
            lblThoiGianThue.Text = h.DonVi;
            calcNumDay();
            loadListServices();
            c.TienDichVu = data.calTotalMoneys(h.MaHD);
            c.TongTien = c.TienDichVu + c.TienPhong;
            lblTongTienDV.Text = c.TienDichVu.ToString();
            lblTongTien.Text = c.TongTien.ToString();
        }

        private void loadListServices()
        {
            if (h != null)
            {
                dgvDichVu.DataSource = data.getListServices(h.MaHD);
                dgvDichVu.Columns["TenDV"].HeaderText = "Tên Dịch Vụ";
                dgvDichVu.Columns["SoLuong"].HeaderText = "Số lượng";
                dgvDichVu.Columns["Gia"].HeaderText = "Đơn giá";
            }
        }

        private void calcNumDay()
        {
            GiaPhong g = data.getPrice(h.SoPhong);
            DateTime dateIn = DateTime.Parse(h.NgayVao);
            DateTime dateOut = DateTime.Parse(h.NgayRa);
            DateTime currentDate = DateTime.Now;

            switch (h.DonVi)
            {
                case "Ngày":
                    if ((Convert.ToInt32(parseTime(currentDate)[3]) <= 12 && currentDate.CompareTo(dateIn) < 0) || currentDate.CompareTo(dateIn) < 0)
                    {
                        lblThoiGianThue.Text = h.ThoiGianThue + " ngày";
                    }
                    else
                    {
                        h.ThoiGianThue = h.ThoiGianThue + 1;
                        lblThoiGianThue.Text = h.ThoiGianThue + " ngày";

                    }
                    lblTienPhong.Text = (g.GiaTheoNgay * h.ThoiGianThue).ToString();
                    c.TienPhong = (g.GiaTheoNgay * h.ThoiGianThue + h.PhuThu);
                    lblTongTienPhong.Text = c.TienPhong.ToString();
                    lblPhuThu.Text = h.PhuThu.ToString();
                    break;
                case "Giờ":
                    int y = Convert.ToInt32(parseTime(currentDate)[3]) - Convert.ToInt32(parseTime(dateOut)[3]);
                    if (y == 0 && Convert.ToInt32(parseTime(currentDate)[3]) > 30)
                        h.ThoiGianThue = h.ThoiGianThue + 1;
                    if (y > 0)
                        h.ThoiGianThue = h.ThoiGianThue + y;

                    if (h.ThoiGianThue == 1)
                    {
                        lblThoiGianThue.Text = "1 giờ";
                        lblTienPhong.Text = g.GiaGioDau.ToString();
                        c.TienPhong = g.GiaGioDau;
                        lblTongTienPhong.Text = c.TienPhong.ToString();
                    }
                    else if (h.ThoiGianThue == 2)
                    {
                        lblThoiGianThue.Text = "2 giờ";
                        lblTienPhong.Text = g.Gia2GioDau.ToString();
                        c.TienPhong = g.Gia2GioDau;
                        lblTongTienPhong.Text = c.TienPhong.ToString();
                    }
                    else if (h.ThoiGianThue == 3)
                    {
                        lblThoiGianThue.Text = "3 giờ";
                        lblTienPhong.Text = g.Gia3GioDau.ToString();
                        c.TienPhong = g.Gia3GioDau;
                        lblTongTienPhong.Text = c.TienPhong.ToString();
                    }
                    else if (h.ThoiGianThue <= 8)
                    {
                        lblThoiGianThue.Text = h.ThoiGianThue.ToString();
                        lblTienPhong.Text = (g.Gia3GioDau + (h.ThoiGianThue - 3) * g.GiaCacGioConLai).ToString();
                        c.TienPhong = g.Gia3GioDau + (h.ThoiGianThue - 3) * g.GiaCacGioConLai;
                        lblTongTienPhong.Text = c.TienPhong.ToString();
                    }
                    else
                    {
                        lblThoiGianThue.Text = "1 ngày";
                        lblTienPhong.Text = g.GiaTheoNgay.ToString();
                        c.TienPhong = g.GiaTheoNgay;
                        lblTongTienPhong.Text = g.GiaTheoNgay.ToString();
                        h.ThoiGianThue = 1;
                        h.DonVi = "Ngày";
                    }
                    break;
                case "Đêm":
                    int z = Convert.ToInt32(parseTime(currentDate)[3]) - 12;
                    if (Convert.ToInt32(parseTime(currentDate)[2]) == Convert.ToInt32(parseTime(dateOut)[2]) && z >= 0)
                    {
                        h.PhuThu = h.PhuThu + g.PhuThu + z * g.PhuThu;
                        lblPhuThu.Text = h.PhuThu.ToString();
                        lblThoiGianThue.Text = "1 đêm";
                        lblTienPhong.Text = g.GiaQuaDem.ToString();
                        lblTongTienPhong.Text = (h.PhuThu + g.GiaQuaDem).ToString();
                        c.TienPhong = h.PhuThu + g.GiaQuaDem;
                    }
                    else
                    {
                        lblPhuThu.Text = h.PhuThu.ToString();
                        lblThoiGianThue.Text = "1 đêm";
                        lblTienPhong.Text = g.GiaQuaDem.ToString();
                        lblTongTienPhong.Text = (h.PhuThu + g.GiaQuaDem).ToString();
                        c.TienPhong = h.PhuThu + g.GiaQuaDem;
                    }
                    break;
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            Close();
        }

        private List<string> parseTime(DateTime d)
        {
            string today = d.ToString("yyyy-MM-dd HH:mm");
            string[] date = today.Split(' ');
            string[] day = date[0].Split('-');
            string[] hour = date[1].Split(':');
            List<string> x = new List<string>();
            foreach (string y in day)
                x.Add(y);
            foreach (string h in hour)
                x.Add(h);
            return x;
        }

        private void lblPhuThu_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            h.TinhTrang = 1;
            h.NgayRa = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            data.pay(h);
            data.addDetail(c);
            MessageBox.Show("Thanh toán thành công! Dữ liệu đã dược lưu!");
            Close();
        }

        private void cbbSoPhong_SelectedValueChanged(object sender, EventArgs e)
        {
            if (h != null)
                formLoad(cbbSoPhong.SelectedValue.ToString());
        }
    }
}
