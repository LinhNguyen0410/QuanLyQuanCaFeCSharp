using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace QuanLyQuanCaFe
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        private void btnExit_Click(object sender, EventArgs e)                //BUTTON THOÁT
        {
            DialogResult xacNhan;
            xacNhan = MessageBox.Show("Bạn có thật sự muốn thoát chương trình?", "Thông Báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (xacNhan == DialogResult.OK)
            {
                Application.Exit();
            }
        }



        private void btnLogin_Click(object sender, EventArgs e)              //BUTTON ĐĂNG NHẬP
        {

            if (txtUser.Text == "" && txtPassWord.Text == "")
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không được trống!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtUser.Text == "")
            {
                MessageBox.Show("Vui lòng không để trống tên đăng nhập!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtPassWord.Text == "")
            {
                MessageBox.Show("Vui lòng không để trống mật khẩu!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (txtUser.Text == "nhom02" && txtPassWord.Text == "123")
                {
                    // Ghi lại tên tài khoản dưới dạng File TXT
                    using (StreamWriter streamWriter = new StreamWriter("tendangnhap.txt"))
                    {
                        streamWriter.WriteLine(txtUser.Text);
                    }

                    Controler obj = new Controler();
                    obj.User = "nhom02";
                    obj.Pass = "123";
                    frmQuanLyBan f = new frmQuanLyBan();
                    this.Hide();
                    f.ShowDialog();
                    this.Show();
                  


                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu sai.Xin vui lòng nhập lại!", "Thông Báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                }
               
                txtPassWord.Clear();
                txtUser.Focus();

            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            // Đọc lại File txt được ghi lên ô tên đăng nhập để ghi nhớ tài khoản.
            using (StreamReader streamReader = new StreamReader("tendangnhap.txt")) 
            {
                txtUser.Text = streamReader.ReadLine();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString(); //Hiển thị thời gian hiện tại
        }
    }
}
    

