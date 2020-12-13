using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT
{
    class HoaDon
    {
        public int MaHD { get; set; }
        public int MaNV { get; set; }
        public int MaKH { get; set; }
        public string SoPhong { get; set; }
        public string NgayVao { get; set; }
        public int ThoiGianThue { get; set; }
        public string DonVi { get; set; }
        public string NgayRa { get; set; }
        public int TinhTrang { get; set; }
        public float PhuThu { get; set; }

        public HoaDon(int MaNV, int MaKH, string SoPhong, string NgayVao, int TinhTrang)
        {
            this.MaNV = MaNV;
            this.MaKH = MaKH;
            this.SoPhong = SoPhong;
            this.NgayVao = NgayVao;
            this.TinhTrang = TinhTrang;
        }
        public HoaDon()
        {

        }
    }
}
