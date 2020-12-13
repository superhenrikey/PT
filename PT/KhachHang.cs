using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT
{
    class KhachHang
    {
        public string HoTen { get; set; }
        public string CMT { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }

        public KhachHang(string Hoten, string CMT, string DiaChi, string SDT)
        {
            this.HoTen = Hoten;
            this.CMT = CMT;
            this.DiaChi = DiaChi;
            this.SDT = SDT;
        }

        public KhachHang()
        {

        }
    }
}
