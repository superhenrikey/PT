using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
namespace PT
{
    class Data
    {
        private SqlConnection conn;
        private SqlDataAdapter adapter;
        private SqlCommand cmd;
        private void ketnoi()
        {
            string connS = ConfigurationManager.ConnectionStrings["connectData"].ToString();
            conn = new SqlConnection(connS);
            conn.Open();
        }
        private void dongketnoi()
        {
            conn.Close();
        }
        public bool checkAccount(string account, string password)
        {
            ketnoi();
            bool x = false;
            cmd = new SqlCommand("SELECT * FROM tblNhanVien WHERE TaiKhoan=@account AND MatKhau=@pass", conn);
            cmd.Parameters.AddWithValue("account", account);
            cmd.Parameters.AddWithValue("pass", password);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                AccountInfo.level = Convert.ToInt32(reader["QuyenTruyCap"]);
                AccountInfo.account = reader["TaiKhoan"].ToString();
                AccountInfo.employeeID = Convert.ToInt32(reader["MaNV"]);
                x = true;
            }
            dongketnoi();
            return x;
        }
        public DataTable getRooms()
        {
            ketnoi();
            adapter = new SqlDataAdapter("SELECT SoPhong,TinhTrang FROM tblPhong", conn);
            DataTable t = new DataTable();
            adapter.Fill(t);
            dongketnoi();
            return t;
        }
        public int[] getRoomsStatus()
        {
            int[] status = new int[3];
            ketnoi();
            cmd = new SqlCommand("SELECT COUNT(*) AS num FROM tblPhong WHERE TinhTrang=0", conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
                status[0] = Convert.ToUInt16(dr["num"]);
            conn.Close();

            conn.Open();
            cmd = new SqlCommand("SELECT COUNT(*) AS num FROM tblPhong WHERE TinhTrang=1", conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
                status[1] = Convert.ToUInt16(dr["num"]);
            conn.Close();

            conn.Open();
            cmd = new SqlCommand("SELECT COUNT(*) AS num FROM tblPhong WHERE TinhTrang=2", conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
                status[2] = Convert.ToUInt16(dr["num"]);
            dongketnoi();
            return status;
        }
        public HoaDon selectBill(string SoPhong)
        {
            HoaDon h = null;
            ketnoi();
            cmd = new SqlCommand("SELECT * FROM tblHoaDon WHERE SoPhong=@SoPhong AND TinhTrang=0", conn);
            cmd.Parameters.AddWithValue("SoPhong", SoPhong);
            SqlDataReader r = cmd.ExecuteReader();
            if (r.Read() && r.HasRows)
            {
                h = new HoaDon()
                {
                    MaHD = Convert.ToInt32(r["MaHD"]),
                    MaNV = Convert.ToInt32(r["MaNV"]),
                    MaKH = Convert.ToInt32(r["MaKH"]),
                    NgayRa = r["NgayRa"].ToString(),
                    NgayVao = r["NgayVao"].ToString(),
                    DonVi = r["DonVi"].ToString(),
                    PhuThu = Convert.ToSingle(r["PhuThu"]),
                    SoPhong = r["SoPhong"].ToString(),
                    ThoiGianThue = Convert.ToInt32(r["ThoiGianThue"]),
                    TinhTrang = Convert.ToInt32(r["TinhTrang"])
                };
            }
            dongketnoi();
            return h;
        }
        public DataTable getListCustomer()
        {
            ketnoi();
            SqlCommand cmd = new SqlCommand("SELECT * FROM tblKhachHang", conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            dongketnoi();
            return dt;
        }
        public DataTable seachPeople(string hoten)
        {
            ketnoi();
            SqlCommand cmd = new SqlCommand("SELECT * FROM tblKhachHang where HoTen=@hoten", conn);
            cmd.Parameters.AddWithValue("hoten", hoten);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            dongketnoi();
            return dt;
        }
        public GiaPhong getPriceType(int LoaiPhong)
        {
            ketnoi();
            GiaPhong g = null;
            string str = "SELECT * FROM tblGia WHERE LoaiPhong=@LoaiPhong";
            cmd = new SqlCommand(str, conn);
            cmd.Parameters.AddWithValue("LoaiPhong", LoaiPhong);
            SqlDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                g = new GiaPhong()
                {
                    LoaiPhong = Convert.ToInt32(r["LoaiPhong"]),
                    GiaGioDau = Convert.ToSingle(r["GiaGioDau"]),
                    Gia2GioDau = Convert.ToSingle(r["Gia2GioDau"]),
                    Gia3GioDau = Convert.ToSingle(r["Gia3GioDau"]),
                    GiaTheoNgay = Convert.ToSingle(r["GiaTheoNgay"]),
                    GiaCacGioConLai = Convert.ToSingle(r["GiaCacGioConLai"]),
                    GiaQuaDem = Convert.ToSingle(r["GiaQuaDem"]),
                    PhuThu = Convert.ToSingle(r["PhuThu"])
                };
            }
            dongketnoi();
            return g;
        }
        public DataTable getRoomType()
        {
            ketnoi();
            adapter = new SqlDataAdapter("SELECT * FROM tblLoaiPhong", conn);
            DataTable t = new DataTable();
            adapter.Fill(t);
            dongketnoi();
            return t;
        }
        public void updatePrice(GiaPhong g)
        {
            ketnoi();
            string query = "UPDATE tblGia SET GiaGioDau=@GiaGioDau,Gia2GioDau=@Gia2GioDau,Gia3GioDau=@Gia3GioDau,GiaCacGioConLai=@GiaCacGioConLai,GiaQuaDem=@GiaQuaDem,GiaTheoNgay=@GiaTheoNgay,PhuThu=@PhuThu WHERE LoaiPhong=@LoaiPhong";
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("GiaGioDau", g.GiaGioDau);
            cmd.Parameters.AddWithValue("Gia2GioDau", g.Gia2GioDau);
            cmd.Parameters.AddWithValue("Gia3GioDau", g.Gia3GioDau);
            cmd.Parameters.AddWithValue("GiaCacGioConLai", g.GiaCacGioConLai);
            cmd.Parameters.AddWithValue("GiaQuaDem", g.GiaQuaDem);
            cmd.Parameters.AddWithValue("GiaTheoNgay", g.GiaTheoNgay);
            cmd.Parameters.AddWithValue("PhuThu", g.PhuThu);
            cmd.Parameters.AddWithValue("LoaiPhong", g.LoaiPhong);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }
        public void clearRoom(string SoPhong)
        {
            ketnoi();
            cmd = new SqlCommand("UPDATE tblPhong SET TinhTrang=1 WHERE SoPhong= @SoPhong", conn);
            cmd.Parameters.AddWithValue("SoPhong", SoPhong);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }

        public void clearDone(string SoPhong)
        {
            ketnoi();
            cmd = new SqlCommand("UPDATE tblPhong SET TinhTrang=0 WHERE SoPhong= @SoPhong", conn);
            cmd.Parameters.AddWithValue("SoPhong", SoPhong);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }
        public GiaPhong getPrice(string roomName)
        {
            ketnoi();
            GiaPhong g = null;
            string str = "SELECT tblPhong.LoaiPhong, GiaGioDau, Gia2GioDau,Gia3GioDau,GiaCacGioConLai,GiaQuaDem,GiaTheoNgay,PhuThu FROM tblPhong JOIN tblGia ON tblPhong.LoaiPhong = tblGia.LoaiPhong WHERE SoPhong=@SoPhong";
            cmd = new SqlCommand(str, conn);
            cmd.Parameters.AddWithValue("SoPhong", roomName);
            SqlDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                g = new GiaPhong()
                {
                    LoaiPhong = Convert.ToInt32(r["LoaiPhong"]),
                    GiaGioDau = Convert.ToSingle(r["GiaGioDau"]),
                    Gia2GioDau = Convert.ToSingle(r["Gia2GioDau"]),
                    Gia3GioDau = Convert.ToSingle(r["Gia3GioDau"]),
                    GiaTheoNgay = Convert.ToSingle(r["GiaTheoNgay"]),
                    GiaCacGioConLai = Convert.ToSingle(r["GiaCacGioConLai"]),
                    GiaQuaDem = Convert.ToSingle(r["GiaQuaDem"]),
                    PhuThu = Convert.ToSingle(r["PhuThu"])
                };
            }
            dongketnoi();
            return g;
        }
        public DataTable getFreeRooms()
        {
            ketnoi();
            adapter = new SqlDataAdapter("SELECT * FROM tblPhong WHERE TinhTrang=0", conn);
            DataTable t = new DataTable();
            adapter.Fill(t);
            dongketnoi();
            return t;
        }
        public void addBill(HoaDon h)  // Đặt hóa đơn
        {
            ketnoi();
            cmd = new SqlCommand("INSERT INTO tblHoaDon VALUES(@MaNV,@MaKH,@SoPhong,@NgayVao,@ThoiGianThue,@DonVi,@NgayRa,0,@PhuThu)", conn);
            cmd.Parameters.AddWithValue("MaNV", h.MaNV);
            cmd.Parameters.AddWithValue("MaKH", h.MaKH);
            cmd.Parameters.AddWithValue("SoPhong", h.SoPhong);
            cmd.Parameters.AddWithValue("NgayVao", h.NgayVao);
            cmd.Parameters.AddWithValue("ThoiGianThue", h.ThoiGianThue);
            cmd.Parameters.AddWithValue("DonVi", h.DonVi);
            cmd.Parameters.AddWithValue("NgayRa", h.NgayRa);
            cmd.Parameters.AddWithValue("PhuThu", h.PhuThu);
            cmd.ExecuteNonQuery();
            dongketnoi();
            changeRoomStatus(h.SoPhong, 2);
        }
        public void changeRoomStatus(string roomID, int stt)
        {
            ketnoi();
            cmd = new SqlCommand("UPDATE tblPhong SET TinhTrang = @TinhTrang WHERE SoPhong=@SoPhong", conn);
            cmd.Parameters.AddWithValue("TinhTrang", stt);
            cmd.Parameters.AddWithValue("SoPhong", roomID);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }
        public void addKhachHang(KhachHang k)
        {
            ketnoi();
            cmd = new SqlCommand("INSERT INTO tblKhachHang VALUES(@name,@id,@address,@phone)", conn);
            cmd.Parameters.AddWithValue("name", k.HoTen);
            cmd.Parameters.AddWithValue("id", k.CMT);
            cmd.Parameters.AddWithValue("address", k.DiaChi);
            cmd.Parameters.AddWithValue("phone", k.SDT);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }
        public int getMaKH(string soCMT)
        {
            ketnoi();
            int maKH = 0;
            cmd = new SqlCommand("select MaKH from tblKhachHang where CMT=@soCMT",conn);
            cmd.Parameters.AddWithValue("soCMT", soCMT);
            SqlDataReader cmdr = cmd.ExecuteReader();
            cmdr.Read();
            maKH = cmdr.GetInt32(0);
            return maKH;
        }
        private string getMaKH2(string soPhong)
        {
            ketnoi();
            string maKH = "";
            cmd = new SqlCommand("select MaKH from tblHoaDon where SoPhong=@sophong", conn);
            cmd.Parameters.AddWithValue("sophong", soPhong);
            SqlDataReader cmdr = cmd.ExecuteReader();
            while (cmdr.Read())
            {
                maKH = cmdr[0].ToString();
            }
            dongketnoi();
            return maKH;
        }
        public KhachHang getThongTinKH(string soPhong)
        {
            string maKH = getMaKH2(soPhong);
            ketnoi();
            KhachHang k = new KhachHang();
            cmd = new SqlCommand("select * from tblKhachHang where MaKH=@ma", conn);
            cmd.Parameters.AddWithValue("ma", maKH);
            using (SqlDataReader cmdr = cmd.ExecuteReader())
            {
                while (cmdr.Read())
                {
                    k.HoTen = cmdr["HoTen"].ToString();
                    k.CMT = cmdr["CMT"].ToString();
                    k.DiaChi = cmdr["DiaChi"].ToString();
                    k.SDT = cmdr["SDT"].ToString();
                }
            }
            dongketnoi();
            return k;
        }
        public string getCustomerName(int CustomerID)
        {
            string name = null;
            ketnoi();
            cmd = new SqlCommand("SELECT HoTen FROM tblKhachHang WHERE MaKH = @MaKH", conn);
            cmd.Parameters.AddWithValue("MaKH", CustomerID);
            SqlDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                name = r["HoTen"].ToString();
            }
            dongketnoi();
            return name;
        }
        public string getEmployeeName(int EmployeeID)
        {
            string name = null;
            ketnoi();
            cmd = new SqlCommand("SELECT HoTen FROM tblNhanVien WHERE MaNV = @MaNV", conn);
            cmd.Parameters.AddWithValue("MaNV", EmployeeID);
            SqlDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                name = r["HoTen"].ToString();
            }
            dongketnoi();
            return name;
        }
        public float calTotalMoneys(int MaHD)
        {
            ketnoi();
            float t = 0;
            cmd = new SqlCommand("SELECT SUM(SoLuong*Gia) AS TongTien, COUNT(*) AS num FROM tblDichVu JOIN tblDanhSachDichVu ON tblDichVu.MaDV=tblDanhSachDichVu.MaDV WHERE MaHD=@MaHD", conn);
            cmd.Parameters.AddWithValue("MaHD", MaHD);
            SqlDataReader r = cmd.ExecuteReader();
            if (r.Read() && r.HasRows)
                if (Convert.ToInt32(r["num"]) > 0)
                    t = Convert.ToSingle(r["TongTien"]);
            dongketnoi();
            return t;
        }
        public DataTable getListServices(int MaHD)
        {
            ketnoi();
            string query = "SELECT TenDV, SoLuong, Gia FROM tblDichVu JOIN tblDanhSachDichVu ON tblDichVu.MaDV=tblDanhSachDichVu.MaDV WHERE MaHD='" + MaHD + "'";
            adapter = new SqlDataAdapter(query, conn);
            DataTable t = new DataTable();
            adapter.Fill(t);
            dongketnoi();
            return t;
        }
        public void pay(HoaDon h)
        {
            ketnoi();
            cmd = new SqlCommand("UPDATE tblHoaDon SET ThoiGianThue=@ThoiGianThue,DonVi=@DonVi,NgayRa=@NgayRa,PhuThu=@PhuThu,TinhTrang=1 WHERE MaHD=@MaHD", conn);
            cmd.Parameters.AddWithValue("MaHD", h.MaHD);
            cmd.Parameters.AddWithValue("ThoiGianThue", h.ThoiGianThue);
            cmd.Parameters.AddWithValue("DonVi", h.DonVi);
            cmd.Parameters.AddWithValue("NgayRa", h.NgayRa);
            cmd.Parameters.AddWithValue("PhuThu", h.PhuThu);
            cmd.ExecuteNonQuery();
            dongketnoi();
            changeRoomStatus(h.SoPhong, 1);
        }
        public void addDetail(ChiTietHoaDon c)
        {
            ketnoi();
            cmd = new SqlCommand("INSERT INTO tblChiTietHoaDon VALUES(@MaHD,@TienPhong,@TienDichVu,@TongTien)", conn);
            cmd.Parameters.AddWithValue("MaHD", c.MaHD);
            cmd.Parameters.AddWithValue("TienPhong", c.TienPhong);
            cmd.Parameters.AddWithValue("TienDichVu", c.TienDichVu);
            cmd.Parameters.AddWithValue("TongTien", c.TongTien);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }
        public DataTable getBusyRooms()
        {
            ketnoi();
            adapter = new SqlDataAdapter("SELECT * FROM tblPhong WHERE TinhTrang=2", conn);
            DataTable t = new DataTable();
            adapter.Fill(t);
            dongketnoi();
            return t;
        }
        public DataTable fillBillServices(int MaHD)
        {
            ketnoi();
            adapter = new SqlDataAdapter("SELECT D.MaDV,TenDV,SoLuong FROM tblDanhSachDichVu AS DS JOIN tblDichVu AS D ON DS.MaDV=D.MaDV WHERE MaHD='" + MaHD + "'", conn);
            DataTable t = new DataTable();
            adapter.Fill(t);
            dongketnoi();
            return t;
        }
        public DataTable fillServiceTable()
        {
            ketnoi();
            adapter = new SqlDataAdapter("SELECT * FROM tblDichVu", conn);
            DataTable t = new DataTable();
            adapter.Fill(t);
            dongketnoi();
            return t;
        }
        public bool serviceExist(int MaHD, int MaDV)
        {
            ketnoi();
            cmd = new SqlCommand("SELECT * FROM tblDanhSachDichVu WHERE MaHD=@MaHD AND MaDV=@MaDV", conn);
            cmd.Parameters.AddWithValue("MaHD", MaHD);
            cmd.Parameters.AddWithValue("MaDV", MaDV);
            SqlDataReader r = cmd.ExecuteReader();
            bool x = r.HasRows;
            dongketnoi();
            return x;
        }

        public void updateServiceNumber(int MaHD, int MaDV, int SoLuong)
        {
            ketnoi();
            cmd = new SqlCommand("UPDATE tblDanhSachDichVu SET SoLuong = SoLuong+@SoLuong WHERE MaHD=@MaHD AND MaDV=@MaDV", conn);
            cmd.Parameters.AddWithValue("MaHD", MaHD);
            cmd.Parameters.AddWithValue("MaDV", MaDV);
            cmd.Parameters.AddWithValue("SoLuong", SoLuong);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }

        public void addServiceNumber(int MaHD, int MaDV, int SoLuong)
        {
            ketnoi();
            cmd = new SqlCommand("INSERT INTO tblDanhSachDichVu VALUES(@MaHD,@MaDV,@SoLuong)", conn);
            cmd.Parameters.AddWithValue("MaHD", MaHD);
            cmd.Parameters.AddWithValue("MaDV", MaDV);
            cmd.Parameters.AddWithValue("SoLuong", SoLuong);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }
        public DataTable fillStatiscical()
        {
            ketnoi();
            string query = "SELECT HD.MaHD,NV.HoTen AS TenNV,KH.HoTen AS TenKH,SoPhong,NgayVao,NgayRa,ThoiGianThue,DonVi,TienPhong,TienDichVu,TongTien FROM tblNhanVien AS NV JOIN tblHoaDon AS HD ON NV.MaNV=HD.MaNV JOIN tblKhachHang AS KH ON HD.MaKH=KH.MaKH JOIN tblChiTietHoaDon AS CTHD ON HD.MaHD = CTHD.MaHD";
            adapter = new SqlDataAdapter(query, conn);
            DataTable t = new DataTable();
            adapter.Fill(t);
            dongketnoi();
            return t;
        }
        public NhanVien getEmployeeInfo(int EmployeeID)
        {
            NhanVien n = null;
            ketnoi();
            cmd = new SqlCommand("SELECT * FROM tblNhanVien WHERE MaNV = @MaNV", conn);
            cmd.Parameters.AddWithValue("MaNV", EmployeeID);
            SqlDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                n = new NhanVien()
                {
                    HoTen = r["HoTen"].ToString(),
                    NgaySinh = r["NgaySinh"].ToString(),
                    DiaChi = r["DiaChi"].ToString(),
                    SDT = r["SDT"].ToString()
                };
            }
            dongketnoi();
            return n;
        }
        public void updateInfo(NhanVien n)
        {
            ketnoi();
            cmd = new SqlCommand("UPDATE tblNhanVien SET HoTen=@HoTen,NgaySinh=@NgaySinh,DiaChi=@DiaChi,SDT=@SDT WHERE MaNV=@MaNV", conn);
            cmd.Parameters.AddWithValue("HoTen", n.HoTen);
            cmd.Parameters.AddWithValue("NgaySinh", n.NgaySinh);
            cmd.Parameters.AddWithValue("DiaChi", n.DiaChi);
            cmd.Parameters.AddWithValue("SDT", n.SDT);
            cmd.Parameters.AddWithValue("MaNV", n.MaNV);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }
        public void changePassword(string account, string password)
        {
            ketnoi();
            cmd = new SqlCommand("UPDATE tblNhanVien SET MatKhau=@pass WHERE TaiKhoan=@account", conn);
            cmd.Parameters.AddWithValue("pass", password);
            cmd.Parameters.AddWithValue("account", account);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }
        public void addService(DichVu d)
        {
            ketnoi();
            cmd = new SqlCommand("INSERT INTO tblDichVu VALUES(@TenDV,@Gia,@DonVi)", conn);
            cmd.Parameters.AddWithValue("TenDV", d.TenDV);
            cmd.Parameters.AddWithValue("Gia", d.Gia);
            cmd.Parameters.AddWithValue("DonVi", d.DonVi);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }
        public void editService(DichVu d)
        {
            ketnoi();
            cmd = new SqlCommand("UPDATE tblDichVu SET TenDV=@TenDV,Gia=@Gia,DonVi=@DonVi WHERE MaDV = @MaDV", conn);
            cmd.Parameters.AddWithValue("TenDV", d.TenDV);
            cmd.Parameters.AddWithValue("Gia", d.Gia);
            cmd.Parameters.AddWithValue("DonVi", d.DonVi);
            cmd.Parameters.AddWithValue("MaDV", d.MaDV);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }

        public void deleteSevice(int MaDV)
        {
            ketnoi();
            cmd = new SqlCommand("DELETE FROM tblDichVu WHERE MaDV=@MaDV");
            cmd.Parameters.AddWithValue("MaDV", MaDV);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }

        public DataTable fillLevelCBB()
        {
            ketnoi();
            adapter = new SqlDataAdapter("SELECT * FROM tblChucVu", conn);
            DataTable t = new DataTable();
            adapter.Fill(t);
            dongketnoi();
            return t;
        }

        public DataTable fillLevel2CBB()
        {
            ketnoi();
            adapter = new SqlDataAdapter("SELECT * FROM tblChucVu WHERE Cap=3", conn);
            DataTable t = new DataTable();
            adapter.Fill(t);
            dongketnoi();
            return t;
        }

        public DataTable fillEmployees()
        {
            ketnoi();
            adapter = new SqlDataAdapter("SELECT MaNV,HoTen,NgaySinh,DiaChi,SDT,TaiKhoan,MoTa FROM tblNhanVien JOIN tblChucVu ON tblNhanVien.QuyenTruyCap = tblChucVu.Cap", conn);
            DataTable t = new DataTable();
            adapter.Fill(t);
            dongketnoi();
            return t;
        }

        public void addEmployee(NhanVien n)
        {
            ketnoi();
            cmd = new SqlCommand("INSERT INTO tblNhanVien VALUES(@HoTen,@NgaySinh,@DiaChi,@SDT,@TaiKhoan,@MatKhau,@QuyenTruyCap)", conn);
            cmd.Parameters.AddWithValue("HoTen", n.HoTen);
            cmd.Parameters.AddWithValue("NgaySinh", n.NgaySinh);
            cmd.Parameters.AddWithValue("DiaChi", n.DiaChi);
            cmd.Parameters.AddWithValue("SDT", n.SDT);
            cmd.Parameters.AddWithValue("TaiKhoan", n.TaiKhoan);
            cmd.Parameters.AddWithValue("MatKhau", n.MatKhau);
            cmd.Parameters.AddWithValue("QuyenTruyCap", n.QuyenTruyCap);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }

        public void editEmployee(NhanVien n)
        {
            ketnoi();
            cmd = new SqlCommand("UPDATE tblNhanVien SET HoTen=@HoTen,NgaySinh=@NgaySinh,DiaChi=@DiaChi,SDT=@SDT,TaiKhoan=@TaiKhoan,MatKhau=@MatKhau,QuyenTruyCap=@QuyenTruyCap WHERE MaNV=@MaNV", conn);
            cmd.Parameters.AddWithValue("HoTen", n.HoTen);
            cmd.Parameters.AddWithValue("NgaySinh", n.NgaySinh);
            cmd.Parameters.AddWithValue("DiaChi", n.DiaChi);
            cmd.Parameters.AddWithValue("SDT", n.SDT);
            cmd.Parameters.AddWithValue("TaiKhoan", n.TaiKhoan);
            cmd.Parameters.AddWithValue("MatKhau", n.MatKhau);
            cmd.Parameters.AddWithValue("QuyenTruyCap", n.QuyenTruyCap);
            cmd.Parameters.AddWithValue("MaNV", n.MaNV);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }

        public void deleteEmployee(int MaNV)
        {
            ketnoi();
            cmd = new SqlCommand("DELETE FROM tblNhanVien WHERE MaNV=@MaNV", conn);
            cmd.Parameters.AddWithValue("MaNV", MaNV);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }
        public void addRoom(Phong p)
        {
            ketnoi();
            cmd = new SqlCommand("INSERT INTO tblPhong VALUES(@SoPhong,@LoaiPhong,0)", conn);
            cmd.Parameters.AddWithValue("SoPhong", p.SoPhong);
            cmd.Parameters.AddWithValue("LoaiPhong", p.LoaiPhong);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }

        public void editRoom(string SoPhong, Phong p)
        {
            ketnoi();
            cmd = new SqlCommand("UPDATE tblPhong SET SoPhong=@SoPhong,LoaiPhong=@LoaiPhong WHERE SoPhong = @n", conn);
            cmd.Parameters.AddWithValue("SoPhong", p.SoPhong);
            cmd.Parameters.AddWithValue("LoaiPhong", p.LoaiPhong);
            cmd.Parameters.AddWithValue("n", SoPhong);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }

        public void deleteRoom(string SoPhong)
        {
            ketnoi();
            cmd = new SqlCommand("DELETE FROM tblPhong WHERE SoPhong=@SoPhong", conn);
            cmd.Parameters.AddWithValue("SoPhong", SoPhong);
            cmd.ExecuteNonQuery();
            dongketnoi();
        }
        public DataTable fillRooms()
        {
            ketnoi();
            adapter = new SqlDataAdapter("SELECT tblPhong.LoaiPhong,SoPhong, MoTa, TinhTrang FROM tblPhong JOIN tblLoaiPhong ON tblPhong.LoaiPhong=tblLoaiPhong.LoaiPhong", conn);
            DataTable t = new DataTable();
            adapter.Fill(t);
            dongketnoi();
            return t;
        }
    }
}
