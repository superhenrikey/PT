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
    public partial class frmDatPhong : Form
    {
        public frmDatPhong()
        {
            InitializeComponent();
        }
        public string SoPhong = null;
        Data data = new Data();
        HoaDon h = new HoaDon();


        private void DatPhong_Load(object sender, EventArgs e)
        {
            cbbSoPhong.DataSource = data.getFreeRooms();
            if (SoPhong != null)
            {
                lblRoom.Text = SoPhong;
                cbbSoPhong.Visible = false;
            }
            else
            {
                lblRoom.Visible = false;
            }
            lblNgayVao.Text = DateTime.Today.ToString("dd-MM-yyyy");
            lblGioVao.Text = DateTime.Now.ToString("HH:mm");
            
            cbbSoPhong.DisplayMember = "SoPhong";
            cbbSoPhong.ValueMember = "SoPhong";
            cbb.TextChanged += new EventHandler(numericUpDown1_TextChanged);
            lblNgayRa.Text = DateTime.Today.AddDays(1).ToString("dd-MM-yyyy");
            lblGioRa.Text = "12:00";
        }




        private void btnDatPhong_Click(object sender, EventArgs e)
        {
            try
            {
                KhachHang k = new KhachHang();
                k.HoTen = txtTen.Text;
                k.CMT = txtCMT.Text;
                k.DiaChi = txtDiaChi.Text;
                k.SDT = txtSDT.Text;
                data.addKhachHang(k);
                ///
                try
                {
                    h.MaKH = data.getMaKH(txtCMT.Text);
                    h.MaNV = AccountInfo.employeeID;
                    if (SoPhong != null)
                        h.SoPhong = lblRoom.Text;
                    else
                        h.SoPhong = cbbSoPhong.SelectedValue.ToString();
                    h.NgayVao = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    h.NgayRa = convertToSQLDateTime(lblNgayRa.Text) + " " + lblGioRa.Text;
                    h.ThoiGianThue = (int)cbb.Value;
                    h.PhuThu = 0;
                    if (radNgay.Checked)
                    {
                        h.DonVi = "Ngày";
                        int x = 12 - Convert.ToInt32(parseTime(DateTime.Now)[3]);
                        if (x > 0)
                        {
                            h.PhuThu = x * data.getPrice(h.SoPhong).PhuThu;
                        }
                    }
                    else if (radGio.Checked)
                    {
                        h.DonVi = "Giờ";
                    }
                    else
                    {
                        h.DonVi = "Đêm";
                        h.ThoiGianThue = 1;
                        int x = 1;
                        DateTime dOut = DateTime.Parse(h.NgayRa);
                        if (Convert.ToInt32(parseTime(dOut)[2]) != Convert.ToInt32(parseTime(DateTime.Now)[2]))
                        {
                            x = 22 - Convert.ToInt32(parseTime(DateTime.Now)[3]);
                            if (x > 0)
                            {
                                h.PhuThu = x * data.getPrice(h.SoPhong).PhuThu;
                            }
                        }
                        if (Convert.ToInt32(parseTime(DateTime.Now)[4]) > 30 && x == 1)
                        {
                            h.PhuThu = 0;
                        }
                    }
                    data.addBill(h);
                    MessageBox.Show("Đặt phòng thành công!");
                    Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi không thể đặt phòng!", "Error");
                }
            }
            catch (ConstraintException)
            {
                MessageBox.Show("Khách hàng đã tồn tại!");
            }
            catch (Exception)
            {
                MessageBox.Show("Đã có lỗi xảy ra!");
            }
        }



        private string convertToSQLDateTime(string str)
        {
            string[] date = str.Split('-');
            return date[2] + "-" + date[1] + "-" + date[0];
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (radNgay.Checked)
            {
                DateTime d = DateTime.Today.Date.AddDays(Convert.ToDouble(cbb.Value));
                lblNgayRa.Text = d.ToString("dd-MM-yyyy");
                lblGioRa.Text = DateTime.Now.ToString("HH:mm");
                lblGioRa.Text = "12:00";
            }
            if (radGio.Checked)
            {
                DateTime h = DateTime.Now.AddHours(Convert.ToDouble(cbb.Value));
                lblGioRa.Text = h.ToString("HH:mm");
            }
        }

        private void numericUpDown1_Leave(object sender, EventArgs e)
        {
            if (radNgay.Checked)
            {
                DateTime d = DateTime.Today.Date.AddDays(Convert.ToDouble(cbb.Value));
                if (Convert.ToInt32(parseTime(DateTime.Now)[3]) < 12)
                {
                    d = d.AddDays(-1);
                }
                lblNgayRa.Text = d.ToString("dd-MM-yyyy");
                lblGioRa.Text = "12:00";
            }
            if (radGio.Checked)
            {
                DateTime h = DateTime.Now.AddHours(Convert.ToDouble(cbb.Value));
                lblGioRa.Text = h.ToString("HH:mm");
            }
        }
        /*Sự kiện khi thay đổi số của Numericbox*/
        void numericUpDown1_TextChanged(object sender, EventArgs e)
        {
            if (radNgay.Checked)
            {
                DateTime d = DateTime.Today.Date.AddDays(Convert.ToDouble(cbb.Value));
                lblNgayRa.Text = d.ToString("dd-MM-yyyy");
                lblGioRa.Text = "12:00";
            }
            if (radGio.Checked)
            {
                DateTime h = DateTime.Now.AddHours(Convert.ToDouble(cbb.Value));
                lblGioRa.Text = h.ToString("HH:mm");
            }
        }
        /*Sự kiện tích vào ô theo ngày*/
        private void radDate_CheckedChanged(object sender, EventArgs e)
        {
            cbb.Enabled = true;
            if (radNgay.Checked)
            {
                DateTime d = DateTime.Today.Date.AddDays(Convert.ToDouble(cbb.Value));
                lblNgayRa.Text = d.ToString("dd-MM-yyyy");
                lblGioRa.Text = "12:00";
            }
        }
        /*Sự kiện tích vào ô Theo giờ*/
        private void radHour_CheckedChanged(object sender, EventArgs e)
        {
            cbb.Enabled = true;
            if (radGio.Checked)
            {
                DateTime h = DateTime.Now.AddHours(Convert.ToDouble(cbb.Value));
                lblGioRa.Text = h.ToString("HH:mm");
                lblNgayRa.Text = h.ToString("dd-MM-yyyy");
            }
        }
        /*Sự kiện tích vào ô Qua đêm*/
        private void radOverNight_CheckedChanged(object sender, EventArgs e)
        {
            cbb.Enabled = false;
            lblGioRa.Text = "12:00";
            if (Convert.ToInt32(parseTime(DateTime.Now)[3]) >= 0 && Convert.ToInt32(parseTime(DateTime.Now)[3]) < 6)
            {
                lblNgayRa.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }
            else
            {
                DateTime t = DateTime.Today.AddDays(1);
                lblNgayRa.Text = t.ToString("dd-MM-yyyy");
            }
        }

        /*Tách thời gian thành một mảng: Time = {năm,tháng,ngày,giờ,phút}*/
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

        private void ThemKH()
        {
            try
            {
                KhachHang k = new KhachHang();
                k.HoTen = txtTen.Text;
                k.CMT = txtCMT.Text;
                k.DiaChi = txtDiaChi.Text;
                k.SDT = txtSDT.Text;
                data.addKhachHang(k);
                Close();
            }
            catch (ConstraintException)
            {
                MessageBox.Show("Khách hàng đã tồn tại!");
            }
            catch (Exception)
            {
                MessageBox.Show("Đã có lỗi xảy ra!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(data.getMaKH("123456789").ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("lỗi");
            }
        }
    }
}
