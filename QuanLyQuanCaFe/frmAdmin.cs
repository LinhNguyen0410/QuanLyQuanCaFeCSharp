using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;

namespace QuanLyQuanCaFe
{
    public partial class frmAdmin : Form
    {
        public frmAdmin()
        {
            InitializeComponent();

        }
        //khai báo DB
        dbQLQuanCaFeDataContext db = new dbQLQuanCaFeDataContext();

        //LOAD DB
        private void frmAdmin_Load(object sender, EventArgs e)
        {

            //Đổ dữ liệu từ textbox tên danh mục (tab DanhMuc)sang combobox tên danh mục(tab Món)
            cbbIDCategory.DataSource = db.DanhMucHangs.OrderBy(p => p.MaDanhMuc);
            cbbIDCategory.DisplayMember = "MaDanhMuc"; // hien thi ndung cho nguoi dung
            cbbIDCategory.ValueMember = "TenDanhMuc"; // hien thi noi dung ben trong
            // load db các TAB
            loadEmploy();
            loadCategory();
            loadFood();

        }


        //xac định dòng đag click  => đổ thông tin từ dtgv qua các txt
        private void dtgvFood_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int dongdangchon = e.RowIndex;
            if (dongdangchon >= 0)
            {
                string maMon = dtgvFood.Rows[dongdangchon].Cells[0].Value.ToString();
                Mon m = db.Mons.Where(p => p.MaMon == maMon).SingleOrDefault();

                //hien thi du lieu len control
                txtIDFood.Text = m.MaMon;
                txtFoodName.Text = m.TenMon;
                txtPrice.Text = m.DonGia.ToString();

            }
        }

        private void cbbIDCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy tên danh mục đang chọn
            string tenDanhMuc = cbbIDCategory.SelectedValue.ToString();

            // hiển thị ds món lên dtgv theo mã danh mục - 01 trả về ds đồ uống - 02 trả về ds đồ ăn
            loadDSMonTheoCbb(tenDanhMuc);

        }

        private void loadDSMonTheoCbb(string tenDanhMuc)
        {
            dtgvFood.DataSource = db.Mons.Where(p => p.DanhMucHang.TenDanhMuc == tenDanhMuc).OrderBy(p => p.MaDanhMuc)
            .Select(p => new { p.MaMon, p.TenMon, p.DonGia });
        }

       


        /// TAB MÓN

        private void loadFood()
        {
            dtgvFood.DataSource = db.Mons.Select(p => new { p.MaMon,p.MaDanhMuc, p.TenMon, p.DonGia });
        }
        //Thêm món
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            if (db.Mons.Where(p => p.MaMon == txtIDFood.Text).SingleOrDefault() != null)
            {
                // tên món có rồi k thêm dc
                MessageBox.Show("Mã món đã tồn tài.Vui lòng nhập lại.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // lấy dữ liệu từ control
            Mon m = new Mon();
            m.MaMon = txtIDFood.Text.Trim();
            m.TenMon = txtFoodName.Text.Trim();
            m.DonGia = int.Parse(txtPrice.Text.Trim());
            m.MaDanhMuc = cbbIDCategory.Text.Trim();

            // nap dữ liệu
            db.Mons.InsertOnSubmit(m);
            db.SubmitChanges();
            // load lai ds mon theo ma danh muc
            MessageBox.Show("Thêm món thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            loadDSMonTheoCbb(cbbIDCategory.SelectedValue.ToString());
        }


        //Xóa món
        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            // lấy dữ liệu
            string maMon = txtIDFood.Text.Trim();
            Mon m = db.Mons.Where(p => p.MaMon == maMon).SingleOrDefault();
            if (m != null)
            if (DialogResult.Yes == MessageBox.Show("Bạn có muốn xóa món này không?", "Xác Nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                // t khác null có nghĩa là đã tồn tại mã món này => xoa dc
                db.Mons.DeleteOnSubmit(m);
                db.SubmitChanges();
                MessageBox.Show("Xóa món thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadFood();

            }
            else
            {
                // ngc lại n chưa tồn tài => k xóa dc
                MessageBox.Show("Món không tồn tại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        //Sửa món
        private void btnEditFood_Click(object sender, EventArgs e)
        {
            // lấy dữ liệu
            string maMon = txtIDFood.Text.Trim();
            Mon m = db.Mons.Where(p => p.MaMon == maMon).SingleOrDefault();
            if (m != null)
            {
                // t khác null có nghĩa là đã tồn tại món này => update dc
                m.MaMon = txtIDFood.Text.Trim();
                
                m.TenMon = txtFoodName.Text.Trim();
                m.DonGia = int.Parse(txtPrice.Text.Trim());

                db.SubmitChanges();   // lênh bắt buộc lưu 1 thay đổi
                MessageBox.Show("Cập nhật thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadFood();

            }
            else
            {
                // ngc lại n chưa tồn tài => k UPDATE dc
                MessageBox.Show("Mã món không tồn tại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        // Tìm Món
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            dtgvFood.DataSource = null;
            dtgvFood.DataSource = db.Mons.Where(p => p.TenMon.Contains(txtFoodName.Text)).OrderBy(p => p.MaDanhMuc)
                .Select(p => new { p.MaMon, p.TenMon, p.DonGia });
        }

        // Làm mới
        private void btnRefreshFood_Click(object sender, EventArgs e)
        {
            loadFood();
        }

        //TAB NHÂN VIÊN
        private void loadEmploy()
            {
                dtgvEmploy.DataSource = db.NhanViens.Select(p => new { p.MaNhanVien, p.TenNhanVien, p.NgaySinh, p.ViTri });
            }

            //thêm nhân viên
            private void btnAddEmploy_Click_1(object sender, EventArgs e)
            {
                if (db.NhanViens.Where(p => p.MaNhanVien == txtEmployID.Text).SingleOrDefault() != null)
                {
                    // tên nhân viên có rồi k thêm dc
                    MessageBox.Show("Mã nhân viên đã tồn tài.Vui lòng nhập lại.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // lấy dữ liệu từ control
                NhanVien t = new NhanVien();
                t.MaNhanVien = txtEmployID.Text.Trim();
                t.TenNhanVien = txtEmployName.Text.Trim();
                t.ViTri = cbbPosition.Text.Trim();
                t.NgaySinh = dtpDateOfBirth.Value;

                // nap dữ liệu
                db.NhanViens.InsertOnSubmit(t);
                db.SubmitChanges();
            // load lai ds
            MessageBox.Show("Thêm nhân viên thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            loadEmploy();

            }


            //xóa nhan viên
            private void btnDeleteEmploy_Click(object sender, EventArgs e)
            {
                // lấy dữ liệu
                string maNhanVien = txtEmployID.Text.Trim();
                NhanVien t = db.NhanViens.Where(p => p.MaNhanVien == maNhanVien).SingleOrDefault();
                if (DialogResult.Yes == MessageBox.Show("Bạn có chắc chắn muốn xóa thông tin nhân viên này không?", "Xác Nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                if (t != null)
                {
                    // t khác null có nghĩa là đã tồn tại nhân viên này => xoa dc
                    db.NhanViens.DeleteOnSubmit(t);
                    db.SubmitChanges();   // lênh bắt buộc lưu 1 thay đổi
                    MessageBox.Show("Xóa nhân viên thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadEmploy();

                }
                else
                {
                    // ngc lại n chưa tồn tài => k xóa dc
                    MessageBox.Show("Mã nhân viên không tồn tại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }


            //sửa nhân viên
            private void btnEditEmploy_Click(object sender, EventArgs e)
            {
                // lấy dữ liệu
                string maNhanVien = txtEmployID.Text.Trim();
                NhanVien t = db.NhanViens.Where(p => p.MaNhanVien == maNhanVien).SingleOrDefault();
                if (t != null)
                {
                    // t khác null có nghĩa là đã tồn tại nhân vien này => update dc
                    t.MaNhanVien = txtEmployID.Text.Trim();
                    t.TenNhanVien = txtEmployName.Text.Trim();
                    t.ViTri = cbbPosition.Text.Trim();
                    t.NgaySinh = dtpDateOfBirth.Value;

                    db.SubmitChanges();   // lênh bắt buộc lưu 1 thay đổi
                    MessageBox.Show("Cập nhật thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadEmploy();

                }
                else
                {
                    // ngc lại n chưa tồn tài => k UPDATE dc
                    MessageBox.Show("Mã nhân viên không tồn tại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

        // Tìm nhân viên
        private void btnSearchEmploy_Click(object sender, EventArgs e)
        {
            dtgvEmploy.DataSource = null;
            dtgvEmploy.DataSource = db.NhanViens.Where(p => p.TenNhanVien.Contains(txtEmployName.Text)).OrderBy(p => p.MaNhanVien).Select(p => new
            { p.MaNhanVien, p.TenNhanVien, p.NgaySinh, p.ViTri }); ;
        }
       

        //xac định dòng đag click  => đổ thông tin từ dtgv qua txt
        private void dtgvEmploy_CellClick(object sender, DataGridViewCellEventArgs e)
            {

                int dongdangchon = e.RowIndex;
                if (dongdangchon >= 0)
                {
                    string maNhanVien = dtgvEmploy.Rows[dongdangchon].Cells[0].Value.ToString();

                    NhanVien t = db.NhanViens.Where(p => p.MaNhanVien == maNhanVien).SingleOrDefault();

                    //hien thi du lieu len control
                    txtEmployID.Text = t.MaNhanVien;
                    txtEmployName.Text = t.TenNhanVien;
                    cbbPosition.Text = t.ViTri;
                    dtpDateOfBirth.Value = t.NgaySinh;
                }
            }

        // Lam Moi
        private void btnRefreshEmploy_Click(object sender, EventArgs e)
        {
            loadEmploy();
        }




        //TAB DANH MỤC SP
        private void loadCategory()
            {
                dtgvCategory.DataSource = db.DanhMucHangs.Select(p => new { p.MaDanhMuc, p.TenDanhMuc });
            }

            //Thêm danh mục sp
            private void btnAddCategory_Click(object sender, EventArgs e)
            {
                if (db.DanhMucHangs.Where(p => p.MaDanhMuc == txtCategoryID.Text).SingleOrDefault() != null)
                {
                    // tên danh mục có rồi k thêm dc
                    MessageBox.Show("Mã danh mục hàng này đã tồn tài.Vui lòng nhập lại.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DanhMucHang d = new DanhMucHang();
                d.MaDanhMuc = txtCategoryID.Text.Trim();
                d.TenDanhMuc = txtCategoryName.Text.Trim();

                //nap du lieu
                db.DanhMucHangs.InsertOnSubmit(d);
                db.SubmitChanges();

                //load lai ds
                loadCategory();
            }

            //Xóa danh mục SP
            private void btnDeleteCategory_Click(object sender, EventArgs e)
            {
                // lấy dữ liệu
                string maDanhMuc = txtCategoryID.Text.Trim();
                DanhMucHang d = db.DanhMucHangs.Where(p => p.MaDanhMuc == maDanhMuc).SingleOrDefault();
                if (d != null)
                {
                    // d khác null có nghĩa là đã tồn tại danh muc này => xoa dc
                    db.DanhMucHangs.DeleteOnSubmit(d);
                    db.SubmitChanges();   // lênh bắt buộc lưu 1 thay đổi
                    MessageBox.Show("Xóa danh mục thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadCategory();

                }
                else
                {
                    // ngc lại n chưa tồn tài => k xóa dc
                    MessageBox.Show("Mã danh mục không tồn tại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            // Sửa danh mục SP
            private void btnEditCategory_Click(object sender, EventArgs e)
            {
                // lấy dữ liệu
                string maDanhMuc = txtCategoryID.Text.Trim();
                DanhMucHang d = db.DanhMucHangs.Where(p => p.MaDanhMuc == maDanhMuc).SingleOrDefault();
                if (d != null)
                {
                    // d khác null có nghĩa là đã tồn tại danh mục này => update dc
                    d.MaDanhMuc = txtCategoryID.Text.Trim();
                    d.TenDanhMuc = txtCategoryName.Text.Trim();

                    db.SubmitChanges();   // lênh bắt buộc lưu 1 thay đổi
                    MessageBox.Show("Cập nhật thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadCategory();

                }
                else
                {
                    // ngc lại n chưa tồn tài => k UPDATE dc
                    MessageBox.Show("Mã danh mục không tồn tại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
         //xac định dòng đag click  => đổ thông tin từ dtgv qua txt
        private void dtgvCategory_CellClick(object sender, DataGridViewCellEventArgs e)
            {
                int dongdangchon = e.RowIndex;
                if (dongdangchon >= 0)
                {
                    string maDanhMuc = dtgvCategory.Rows[dongdangchon].Cells[0].Value.ToString();

                    DanhMucHang d = db.DanhMucHangs.Where(p => p.MaDanhMuc == maDanhMuc).SingleOrDefault();

                    //hien thi du lieu len control
                    txtCategoryID.Text = d.MaDanhMuc;
                    txtCategoryName.Text = d.TenDanhMuc;
                }
            }
        // Làm mớii
        private void btnRefreshCate_Click(object sender, EventArgs e)
        {
            loadCategory();
        }

       
    }
}










