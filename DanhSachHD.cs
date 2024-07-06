using System;
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
    public partial class DanhSachHD : Form
    {
        private string MaBN;
        private DataView dataViewDSHD = new DataView();
        private DataTable tblDSHD = new DataTable();
        private string connectionString = @"Data Source=DESKTOP-3FLNI2M;Initial Catalog=QLBV;Integrated Security=True";
        public DanhSachHD(string MaBN)
        {
            this.MaBN = MaBN;
            InitializeComponent();
            DisplayDanhSach();
        }
        public void DisplayDanhSach()
        {
            string query = "SELECT hd.Ngay,bn.TenBN,hd.DichVu FROM tblHopDong hd INNER JOIN tblBenhNhan bn ON hd.MaBN = bn.MaBN WHERE hd.MaBN = @MaBN";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, conn))
                {
                    sqlCommand.Parameters.AddWithValue("@MaBN", MaBN);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand))
                    {
                        tblDSHD.Clear(); // Clear existing data if any
                        adapter.Fill(tblDSHD); // Fill the DataTable with results from query
                        dataViewDSHD = tblDSHD.DefaultView;

                        // Assuming dataGridView1 is your DataGridView control
                        dataGridView1.DataSource = dataViewDSHD;


                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridView1.CurrentRow.Index;
            string maBN = dataViewDSHD[index]["TenBN"].ToString();
            string Ngay = dataViewDSHD[index]["Ngay"].ToString();
            string DichVu = dataViewDSHD[index]["DichVu"].ToString();
            Form1 form = new Form1();
            form.ShowDialog();
        }
    }
}
