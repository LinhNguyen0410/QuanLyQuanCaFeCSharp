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
    public partial class frmQuanLyBan : Form
    {
        //khai báo DB
        dbQLQuanCaFeDataContext db = new dbQLQuanCaFeDataContext();
        public frmQuanLyBan()
        {
            InitializeComponent();

            // đổ dữ liệu từ txtCategoryName frmAdmin qua cbbCategory frmQuanLyBan

            cbbCategory.DataSource = db.DanhMucHangs.OrderBy(p => p.TenDanhMuc);
            cbbCategory.DisplayMember = "TenDanhMuc"; // hien thi ndung cho nguoi dung
            cbbCategory.ValueMember = "MaDanhMuc"; // hien thi noi dung ben trong

            loadFoodBill();
            loadDSMonTheoCBB();

        }
        private void loadFoodBill()
        {
            dtgvFoodBill.DataSource = db.ThongTinHoaDons.Where(p => p.MaHoaDon == maHoaDon).OrderBy(p => p.MaHoaDon).Select(p => new { p.MaMon, p.Mon.TenMon, p.SoLuong, p.ThanhTien, p.MaHoaDon });
        }



        //Phân loai món theo tên danh mục - đồ uống - thức ăn nhanh
        private void cbbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadDSMonTheoCBB();
        }

        private void loadDSMonTheoCBB()
        {
            cbbFood.DataSource = db.Mons.Where(p => p.MaDanhMuc == cbbCategory.SelectedValue.ToString());
            cbbFood.DisplayMember = "TenMon";
            cbbFood.ValueMember = "MaMon";
        }




        //Thêm món vào bill
        private void frmQuanLyBan_Load(object sender, EventArgs e)
        {
            maHoaDon = DateTime.Now.ToString("ddMMyyyy hhmmss"); //tạo mã hóa đơn 
        }

        string maHoaDon = "";

        private void btnAddBill_Click(object sender, EventArgs e)
        {
            //check tao hoa don chua
            HoaDon hd = db.HoaDons.Where(p => p.MaHoaDon == maHoaDon).SingleOrDefault();
            if (hd == null)
            {
                //chua tao
                hd = new HoaDon();
                hd.GioVao = DateTime.Now;
                hd.MaHoaDon = maHoaDon;

                db.HoaDons.InsertOnSubmit(hd);
                db.SubmitChanges();
            }

            ThongTinHoaDon tthd = new ThongTinHoaDon();
            tthd.MaHoaDon = maHoaDon;
            tthd.MaMon = cbbFood.SelectedValue.ToString();
            tthd.SoLuong = (int)nudFood.Value;
            tthd.ThanhTien = tthd.SoLuong * int.Parse(txtPrice.Text);
            db.ThongTinHoaDons.InsertOnSubmit(tthd);
            db.SubmitChanges();

            loadFoodBill();


        }


        //Xóa hết bill
        private void btnDeletaAll_Click(object sender, EventArgs e)
        {

            //check tao hoa don chua

            HoaDon hd = db.HoaDons.Where(p => p.MaHoaDon == maHoaDon).SingleOrDefault();
            if (DialogResult.Yes == MessageBox.Show("Bạn có muốn xóa hóa đơn không?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                if (hd != null)
                {
                    List<ThongTinHoaDon> lst = db.ThongTinHoaDons.Where(p => p.MaHoaDon == maHoaDon).ToList();
                    db.ThongTinHoaDons.DeleteAllOnSubmit(lst);

                    db.HoaDons.DeleteOnSubmit(hd);
                    db.SubmitChanges();

                    maHoaDon = DateTime.Now.ToString("ddMMyyyy hhmmss");
                    loadFoodBill();
                }
        }

        // Xóa 1 món trong bill
        private void btnDeleteBill_Click(object sender, EventArgs e)
        {
            int dongdangchon = dtgvFoodBill.CurrentCell.RowIndex;
            string maMon = dtgvFoodBill.Rows[dongdangchon].Cells[0].Value.ToString();
            string maHoaDon = dtgvFoodBill.Rows[dongdangchon].Cells[4].Value.ToString();

            if (DialogResult.Yes == MessageBox.Show("Bạn có muốn xóa món này không?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                ThongTinHoaDon tthd = db.ThongTinHoaDons.Where(p => p.MaMon == maMon && p.MaHoaDon == maHoaDon).FirstOrDefault();
                if (tthd != null)
                {
                    db.ThongTinHoaDons.DeleteOnSubmit(tthd);
                    db.SubmitChanges();

                    loadFoodBill();
                }
            }

        }


        //Cập nhật món

        private void btnEditBill_Click(object sender, EventArgs e)
        {
            int dongdangchon = dtgvFoodBill.CurrentCell.RowIndex;
            string maMon = dtgvFoodBill.Rows[dongdangchon].Cells[0].Value.ToString();
            string maHoaDon = dtgvFoodBill.Rows[dongdangchon].Cells[4].Value.ToString();

            {
                ThongTinHoaDon tthd = db.ThongTinHoaDons.Where(p => p.MaMon == maMon && p.MaHoaDon == maHoaDon).FirstOrDefault();
                if (tthd != null)
                {
                    tthd.SoLuong = (int)nudFood.Value;
                    tthd.ThanhTien = (int)nudFood.Value * tthd.Mon.DonGia;
                    db.SubmitChanges();
                    loadFoodBill();

                }
            }
        }



        // Ấn vào hình đổ dữ liệu món qua dtgv
        private void picCafe_Click(object sender, EventArgs e)
        {
            PictureBox pi = (PictureBox)sender;
            string maMon = pi.Tag.ToString();
            Mon m = db.Mons.Where(p => p.MaMon == maMon).SingleOrDefault();
            HoaDon hd = db.HoaDons.Where(p => p.MaHoaDon == maHoaDon).SingleOrDefault();
            if (hd == null)
            {
                //chua tao
                hd = new HoaDon();
                hd.GioVao = DateTime.Now;
                hd.MaHoaDon = maHoaDon;

                db.HoaDons.InsertOnSubmit(hd);
                db.SubmitChanges();
            }

            ThongTinHoaDon tthd = new ThongTinHoaDon();
            tthd.MaHoaDon = maHoaDon;
            tthd.MaMon = maMon;
            tthd.SoLuong = 1;
            tthd.ThanhTien = tthd.SoLuong * int.Parse(txtPrice.Text);
            db.ThongTinHoaDons.InsertOnSubmit(tthd);
            db.SubmitChanges();

            loadFoodBill();

        }




        //Hiển thị đơn giá của 1 sp lên label
        private void chon_hang_hoa()
        {
            txtPrice.Text = db.Mons.Where(p => p.MaMon == cbbFood.SelectedValue.ToString()).SingleOrDefault()
             .DonGia.ToString();
        }
        //Sự kiện thay đổi chọn hàng hóa
        private void cbbFood_SelectedIndexChanged(object sender, EventArgs e)
        {
            chon_hang_hoa();
        }

        // Tính tổng tiền hóa đơn
        private void lblTotal_Click(object sender, EventArgs e)
        {
            lblTotal.Text = dtgvFoodBill.Rows.Cast<DataGridViewRow>()
                               .AsEnumerable()
                               .Sum(x => int.Parse(x.Cells[3].Value.ToString()))
                               .ToString("#,##0") + " vnđ";
        }


        //In hóa đơn
        private void btnPay_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter("HoaDon_M10COFFEE.txt", true);
            sw.WriteLine("\n----------------------------------------M10 COFFEE--------------------------------------");
            sw.WriteLine("\nTên Nhân Viên: " + txtEmployName.Text);
            sw.WriteLine("\nThời gian: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            sw.WriteLine("\nTổng tiền: " + dtgvFoodBill.Rows.Cast<DataGridViewRow>()
                               .AsEnumerable()
                               .Sum(x => int.Parse(x.Cells[3].Value.ToString()))
                               .ToString("#,##0") + " VND");

            sw.WriteLine("\n\tMã Món       	\tTên Món      	  \tSố lượng	   \tTổngtiền	          \tMã Hóa Đơn");
            sw.WriteLine("\n___________________________________________________________________________________________");
            for (int i = 0; i < dtgvFoodBill.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dtgvFoodBill.Columns.Count; j++)
                {
                    sw.Write("\t" + dtgvFoodBill.Rows[i].Cells[j].Value.ToString() + "\t" + "|");
                }
                sw.WriteLine("\n");

            }
            if (DialogResult.Yes == MessageBox.Show("Bạn có muốn in hóa đơn và thanh toán?", "Xác Nhận",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                MessageBox.Show("In hóa đơn thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            sw.WriteLine("\n\n\n******************************************THANKS YOU!!!***************************************");
            sw.Close();

            

        }





        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin f = new frmAdmin();

            this.Hide();
            f.ShowDialog();
            this.Show();

        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString(); //Hiển thị thời gian hiện tại
        }



        private void đăngXuấtToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DialogResult xacNhan;
            xacNhan = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất ? ", "Xác Nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (xacNhan == DialogResult.OK)
            {
                this.Close();

            }
        }


       


        //MouseHover Món
        private void picCafe_MouseHover_1(object sender, EventArgs e)
        {
            toolTip1.Show("CaFe Đen Đá - 20000VND", picCafe);
        }
        private void picTraSua_MouseHover_1(object sender, EventArgs e)
        {
            toolTip1.Show("Milk Tea - 28000VND", picTraSua);
        }

        private void picCafeSua_MouseHover_1(object sender, EventArgs e)
        {
            toolTip1.Show("CaFe Sữa Đá - 25000VND", picCafeSua);
        }

        private void picSuaNong_MouseHover_1(object sender, EventArgs e)
        {
            toolTip1.Show("CaFe Sữa Nóng - 22000VND", picSuaNong);
        }

        private void picTraChanh_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Trà Chanh - 20000VND", picTraChanh);
        }

        private void picSting_MouseHover_1(object sender, EventArgs e)
        {
            toolTip1.Show("Sting - 20000VND", picSting);
        }

        private void pic7Up_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("7 UP- 20000VND", pic7Up);
        }

        private void picCoca_MouseHover_1(object sender, EventArgs e)
        {
            toolTip1.Show("CoCaCoLa - 20000VND", picCoca);
        }

        private void picPebsi_MouseHover_1(object sender, EventArgs e)
        {
            toolTip1.Show("Pepsi- 20000VND", picPebsi);
        }

        private void picTraDao_MouseHover_1(object sender, EventArgs e)
        {
            toolTip1.Show("Trà Đào - 25000VND", picTraDao);
        }

        private void picSoda_MouseHover_1(object sender, EventArgs e)
        {
            toolTip1.Show("Soda - 20000VND", picSoda);
        }

        private void picPhoMai_MouseHover_1(object sender, EventArgs e)
        {
            toolTip1.Show("Phô Mai Que - 19000VND", picPhoMai);
        }

        private void picBanMi_MouseHover_1(object sender, EventArgs e)
        {
            toolTip1.Show("Bánh Mì Chả Lụa - 20000VND", picBanMi);
        }

        private void picKhoaiTay_MouseHover_1(object sender, EventArgs e)
        {
            toolTip1.Show("Khoai Tây Chiên- 25000VND", picKhoaiTay);

        }

 
    }
}

        

