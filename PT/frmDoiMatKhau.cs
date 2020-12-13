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
    public partial class frmDoiMatKhau : Form
    {
        public frmDoiMatKhau()
        {
            InitializeComponent();
        }
        Data data = new Data();

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (!data.checkAccount(AccountInfo.account, txtPassCu.Text))
            {
                lblError.Text = "Sai mật khẩu!";
            }
            else if ((txtPassMoi.Text != txtPassMoi2.Text))
            {
                lblError.Text = "Mật khẩu mới không khớp!";
            }
            else
            {
                data.changePassword(AccountInfo.account, txtPassMoi.Text);
                MessageBox.Show("Thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }

        private void frmDoiMatKhau_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
        }
    }
}
