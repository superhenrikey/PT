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
    public partial class frmLogin : Form
    {
        Data data = new Data();
        public frmLogin()
        {
            InitializeComponent();
        }
           
        private void closeFLogin(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool check = data.checkAccount(txtUsername.Text, txtPassword.Text);
            if (check)
            {
                frmMain frmMain = new frmMain();
                frmMain.FormClosed += new FormClosedEventHandler(closeFLogin);
                this.Hide();
                frmMain.Show();
            }
            else
                MessageBox.Show("Đăng nhập thất bại");
        }
    }
}
