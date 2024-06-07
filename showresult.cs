using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using System.Xml.Linq;
using static madarsa_student_app.dashboard;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace madarsa_student_app
{
    public partial class showresult : Form
    {
        private string ConnectionString = dashboard.ConnectionString;
        public static string Paper = "";
        public static string KLas = "";
        public static string Nam = "";
        public static string father = "";
        public static string m1 = "";
        public static string m2 = "";
        public static string m3 = "";
        public static string m4 = "";
        public static string m5 = "";
        public static string m6 = "";
        public static string m7 = "";

        public showresult()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();

            string searchText = name.Text;
            string selectedClass = klas.SelectedItem?.ToString();
            string selectedExam = paper.SelectedItem?.ToString(); // Assuming comboBox2 is for selecting the exam/paper

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Base query with join to class and student_admission tables
                string query = @"SELECT r.student_name, sa.fathername, c.class, r.examname, r.s1marks, r.s2marks, 
                            r.s3marks, r.s4marks, r.s5marks, r.s6marks, r.s7marks
                     FROM result r
                     INNER JOIN student_admission sa ON r.student_id = sa.id
                     INNER JOIN class c ON r.class_id = c.id
                     WHERE 1=1";

                // Append conditions based on input
                if (!string.IsNullOrEmpty(searchText))
                {
                    query += " AND (sa.cnic = @searchText OR sa.name = @searchText)";
                }
                if (!string.IsNullOrEmpty(selectedClass))
                {
                    query += " AND c.class = @selectedClass";
                }
                if (!string.IsNullOrEmpty(selectedExam))
                {
                    query += " AND r.examname = @selectedExam";
                }

                SqlCommand command = new SqlCommand(query, connection);

                // Add parameters conditionally
                if (!string.IsNullOrEmpty(searchText))
                {
                    command.Parameters.AddWithValue("@searchText", searchText);
                }
                if (!string.IsNullOrEmpty(selectedClass))
                {
                    command.Parameters.AddWithValue("@selectedClass", selectedClass);
                }
                if (!string.IsNullOrEmpty(selectedExam))
                {
                    command.Parameters.AddWithValue("@selectedExam", selectedExam);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }

            dataGridView1.DataSource = dataTable;

        }
        public void comboboxshow()
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                string query = "SELECT class FROM class";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                klas.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                }
                sqlConnection.Close();
            }

        }

        private void showresult_Load(object sender, EventArgs e)
        {

            comboboxshow();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                   
                     Nam = row.Cells["student_name"].Value?.ToString();
                     Paper = row.Cells["examname"].Value?.ToString();
                     KLas = row.Cells["class"].Value?.ToString();
                     father= row.Cells["fathername"].Value?.ToString();
                     m1 = row.Cells["s1marks"].Value?.ToString();
                     m2 = row.Cells["s2marks"].Value?.ToString();
                     m3 = row.Cells["s3marks"].Value?.ToString();
                     m4 = row.Cells["s4marks"].Value?.ToString();
                     m5 = row.Cells["s5marks"].Value?.ToString();
                     m6 = row.Cells["s6marks"].Value?.ToString();
                     m7 = row.Cells["s7marks"].Value?.ToString();
            }
            result_report s = new result_report();
            s.ShowDialog(this);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
         
        }
       

    }
}
