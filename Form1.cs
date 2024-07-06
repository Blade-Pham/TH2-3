using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TH2_3
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Data Source=DESKTOP-3FLNI2M;Initial Catalog=QLBV;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
            displayMaBN();
            displayDichVu();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
if (cb_mabenhnhan.SelectedValue is DataRowView)
    {
        DataRowView drv = (DataRowView)cb_mabenhnhan.SelectedValue;
        string selectedMaBN = drv["MaBN"].ToString();
        LoadTenBN(selectedMaBN);
    }
    else if (cb_mabenhnhan.SelectedValue != null)
    {
        string selectedMaBN = cb_mabenhnhan.SelectedValue.ToString();
        LoadTenBN(selectedMaBN);
    }
        }
        public void displayMaBN()
        {
            // Define your query
            string query = "SELECT MaBN FROM tblBenhNhan";

            // Create a new SqlConnection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create a new SqlDataAdapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                // Create a new DataTable
                DataTable dataTable = new DataTable();

                try
                {
                    // Open the connection
                    connection.Open();

                    // Fill the DataTable
                    dataAdapter.Fill(dataTable);

                    // Set the ComboBox DataSource
                    cb_mabenhnhan.DataSource = dataTable;

                    // Set the display and value members
                    cb_mabenhnhan.DisplayMember = "MaBN";
                    cb_mabenhnhan.ValueMember = "MaBN";
                }
                catch (Exception ex)
                {
                    // Handle any errors that may have occurred
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
        private void LoadTenBN(string maBN)
        {
            // Define the query to select TenBN from tblBenhNhan where MaBN matches the selected value
            string query = "SELECT TenBN FROM tblBenhNhan WHERE MaBN = @MaBN";

            // Sử dụng SqlConnection và SqlCommand để thực thi truy vấn
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaBN", maBN);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            string tenBN = reader["TenBN"].ToString();
                            tb_tenbenhnhan.Text = tenBN;
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                    }
                }
            }
        }
    

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_dichvu.SelectedValue is DataRowView)
            {
                DataRowView drv = (DataRowView)cb_dichvu.SelectedValue;
                string selectedDichVu = drv["TenDV"].ToString();

                // Thêm dịch vụ đã chọn vào RichTextBox
                if (!string.IsNullOrEmpty(selectedDichVu))
                {
                    // Thêm dịch vụ đã chọn vào RichTextBox
                    richTextBox1.AppendText(selectedDichVu + Environment.NewLine);
                }
            }
            else if (cb_dichvu.SelectedValue != null)
            {
                string selectedDichVu = cb_dichvu.SelectedValue.ToString();

                // Thêm dịch vụ đã chọn vào RichTextBox
                richTextBox1.AppendText(selectedDichVu + Environment.NewLine);
            }
        }
        public void displayDichVu()
        {
            string query = "SELECT TenDV FROM tblDichVu";

            // Create a new SqlConnection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create a new SqlDataAdapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                // Create a new DataTable
                DataTable dataTable = new DataTable();

                try
                {
                    // Open the connection
                    connection.Open();

                    // Fill the DataTable
                    dataAdapter.Fill(dataTable);
                    DataRow emptyRow = dataTable.NewRow();
                    emptyRow["TenDV"] = "";
                    dataTable.Rows.InsertAt(emptyRow, 0);

                    // Set the ComboBox DataSource
                    cb_dichvu.DataSource = dataTable;

                    // Set the display and value members
                    cb_dichvu.DisplayMember = "TenDV";
                    cb_dichvu.ValueMember = "TenDV";
                }
                catch (Exception ex)
                {
                    // Handle any errors that may have occurred
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_nam.Text) || string.IsNullOrEmpty(tb_tenbenhnhan.Text) || string.IsNullOrEmpty(tb_ngay.Text) || string.IsNullOrEmpty(tb_thang.Text))
            {

                MessageBox.Show("Vui lòng nhập đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string MaBN = "";
            if (cb_mabenhnhan.SelectedValue is DataRowView)
            {
                DataRowView drv = (DataRowView)cb_mabenhnhan.SelectedValue;
                MaBN = drv["MaBN"].ToString();
            }
            else if (cb_mabenhnhan.SelectedValue != null)
            {
                MaBN = cb_mabenhnhan.SelectedValue.ToString();
            }
            string name=tb_tenbenhnhan.Text;
            string ngay = tb_ngay.Text;
            string thang = tb_thang.Text;
            string nam = tb_nam.Text;
            string dichvu = richTextBox1.Text;
            int day = int.Parse(tb_ngay.Text);
            int month = int.Parse(tb_thang.Text);
            int year = int.Parse(tb_nam.Text);

            DateTime date = new DateTime(year, month, day);
            richTextBox2.Text = $"Ten Benh Nhanh:{name} \n" +
                    $"Ngay: {ngay}/{thang}/{nam} \n" +
                    $"Dich Vu:{dichvu}";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO tblHopDong(Ngay, MaBN, DichVu) VALUES(@Ngay, @MaBN, @DichVu)", conn);
                    cmd.Parameters.AddWithValue("@MaBN", MaBN);
                    cmd.Parameters.AddWithValue("@Ngay",date );
                    cmd.Parameters.AddWithValue("@DichVu", dichvu);
                    
                    cmd.ExecuteNonQuery();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {
            string MaBN = "";
            if (cb_mabenhnhan.SelectedValue is DataRowView)
            {
                DataRowView drv = (DataRowView)cb_mabenhnhan.SelectedValue;
                MaBN = drv["MaBN"].ToString();
            }
            else if (cb_mabenhnhan.SelectedValue != null)
            {
                MaBN = cb_mabenhnhan.SelectedValue.ToString();
            }

            if (!string.IsNullOrEmpty(MaBN))
            {
                DanhSachHD dshd = new DanhSachHD(MaBN);
                dshd.Show();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn Mã Bệnh Nhân.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
