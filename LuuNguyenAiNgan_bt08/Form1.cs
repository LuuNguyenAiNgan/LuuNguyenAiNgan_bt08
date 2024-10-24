using LuuNguyenAiNgan_bt08.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LuuNguyenAiNgan_bt08
{
    public partial class Form1 : Form
    {
        private readonly Model1 db = new Model1();
        private Student student;
        public Form1()
        {
            InitializeComponent();
            LoadMajors();
            LoadData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'schoolDBDataSet.Students' table. You can move, or remove it, as needed.
            this.studentsTableAdapter.Fill(this.schoolDBDataSet.Students);

        }
        private void LoadData()
        {
            dataGridView1.DataSource = db.Students.ToList();

        }
        private void LoadMajors()
        {
            cmbMajor.Items.Add("Công nghệ thông tin");
            cmbMajor.Items.Add("Kế toán");
            cmbMajor.Items.Add("Ngôn ngữ");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem các trường nhập liệu có hợp lệ không
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) ||
                cmbMajor.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin sinh viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var student = new Student
            {
                FullName = txtFullName.Text,
                Age = int.Parse(txtAge.Text),
                Major = cmbMajor.SelectedItem.ToString()
            };

            // Thêm sinh viên vào cơ sở dữ liệu
            db.Students.Add(student);
            db.SaveChanges();

            // Thông báo thêm thành công
            MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có sinh viên nào được chọn không
            if (student != null)
            {
                // Kiểm tra thông tin nhập vào
                if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                    !int.TryParse(txtAge.Text, out int age) ||
                    cmbMajor.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin: Họ tên, Tuổi và Ngành học.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Cập nhật thông tin sinh viên
                student.FullName = txtFullName.Text;
                student.Age = age;
                student.Major = cmbMajor.SelectedItem.ToString();

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                // Thông báo sửa thành công
                MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadData();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có sinh viên nào được chọn không
            if (student != null)
            {
                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    db.Students.Remove(student);
                    db.SaveChanges();
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa.");
            }
        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                student = (Student)dataGridView1.CurrentRow.DataBoundItem;
                txtFullName.Text = student.FullName;
                txtAge.Text = student.Age.ToString();
                cmbMajor.SelectedItem = student.Major;
            }
        }
    }
}
