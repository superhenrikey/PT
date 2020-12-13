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
    public partial class frmGiaPhong : Form
    {
        public frmGiaPhong()
        {
            InitializeComponent();
        }
        public string SoPhong = null;
        Data data = new Data();
        private void FrmGiaPhong_Load(object sender, EventArgs e)
        {
            if (SoPhong != null)
            {
                GiaPhong g = data.getPrice(SoPhong);
                if (g.LoaiPhong == 1)
                    lblSoPhong.Text = "Phòng " + SoPhong + ", Phòng đơn";
                if (g.LoaiPhong == 2)
                    lblSoPhong.Text = "Phòng " + SoPhong + ", Phòng đôi";
                lblGioDau.Text = "Giá giờ đầu: "+g.GiaGioDau + " VND";
                lbl2GioDau.Text = "Giá 2 giờ đầu: " + g.Gia2GioDau + " VND";
                lbl3GioDau.Text = "Giá 3 giờ đầu: " + g.Gia3GioDau + " VND";
                lblCacGioTiepTheo.Text = "Giá giờ tiếp theo: " + g.GiaCacGioConLai + " VND";
                lblGiaQuaDem.Text = "Giá qua đêm: " + g.GiaQuaDem + " VND";
                lblGiaTheoNgay.Text = "Giá theo ngày: " + g.GiaTheoNgay + " VND";
                lblPhuThu.Text = "Phụ thu: " + g.PhuThu + " VND";
            }
        }
    }
}
