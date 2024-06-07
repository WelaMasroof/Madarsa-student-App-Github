using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static madarsa_student_app.dashboard;

namespace madarsa_student_app
{
    public partial class studentdetails : Form
    {
        public static string sname = "";
        public static string fathername = "";
        public static string address = ""; 
        public static string date = "";
        public static string contact = ""; 
        public static string alterno = ""; 
        public static string gardianno = ""; 
        public static string relationwithgardian = "";
        public static string previousqualification = "";
        public static string sclass = "";
        public static string cnic = "";
        private string ConnectionString = dashboard.ConnectionString;

        public studentdetails()
        {
            InitializeComponent();
        }

        private void studentdetails_Load(object sender, EventArgs e)
        {
            comboboxshow();
        }

        private void loadallstudents()
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT name, fathername, address, date_of_admission, contact_no, alternate_no, guardian_no, relation_with_guardian, previous_qualification, cnic, class FROM student_admission";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }

            dataGridView1.DataSource = dataTable;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
                                comboBox1.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                }
                sqlConnection.Close();
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();

            string searchText = search.Text;
            string selectedClass = comboBox1.SelectedItem?.ToString();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Base query with join to class table
                string query = @"SELECT sa.name, sa.fathername, sa.address, sa.date_of_admission, 
                            sa.contact_no, sa.alternate_no, sa.guardian_no, 
                            sa.relation_with_guardian, sa.previous_qualification, 
                            sa.cnic, c.class 
                     FROM student_admission sa
                     INNER JOIN class c ON sa.class_id = c.id
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

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }

            dataGridView1.DataSource = dataTable;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadallstudents();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select at least one row to delete.", "No Rows Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete the selected row(s)?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Delete selected rows from the database and DataGridView
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                     sname = Convert.ToString(row.Cells["name"].Value);
                     fathername = Convert.ToString(row.Cells["fathername"].Value);
                     cnic = Convert.ToString(row.Cells["cnic"].Value);
                     sclass = Convert.ToString(row.Cells["class"].Value);
                     address = Convert.ToString(row.Cells["address"].Value);
                     date = Convert.ToString(row.Cells["date_of_admission"].Value); 
                     contact = Convert.ToString(row.Cells["contact_no"].Value);
                     alterno = Convert.ToString(row.Cells["alternate_no"].Value);
                     gardianno = Convert.ToString(row.Cells["guardian_no"].Value);
                     relationwithgardian = Convert.ToString(row.Cells["relation_with_guardian"].Value);
                     previousqualification = Convert.ToString(row.Cells["previous_qualification"].Value);


                    // Delete row from database
                    DeleteRowFromDatabase(sname, fathername, cnic, sclass);

                    // Remove row from DataGridView
                    dataGridView1.Rows.Remove(row);
                }

                // Open the studentleft form as a modal dialog
                studentleft s = new studentleft();
                s.ShowDialog(this); // Pass the current form as the owner of the modal dialog
            }
        }
         private void DeleteRowFromDatabase(string name, string fathername, string cnic, string clas)
         {
            // Delete row from database based on the given Idsss
            string query = "DELETE FROM student_admission WHERE name = @sales and fathername=@Category and cnic=@Amount and class=@Description";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@sales",name );
                command.Parameters.AddWithValue("@Category", fathername);
                command.Parameters.AddWithValue("@Amount", cnic);
                command.Parameters.AddWithValue("@Description", clas);
                connection.Open();
                command.ExecuteNonQuery();
            }
         }

        private void search_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
