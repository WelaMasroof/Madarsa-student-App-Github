using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static madarsa_student_app.dashboard;


namespace madarsa_student_app
{
    public partial class left_students : Form
    {
        private string ConnectionString = dashboard.ConnectionString;

        public static string sname = "";
        public static string fathername = "";
        public static string sclass = "";
        public static string cnic = "";
        public left_students()
        {
            InitializeComponent();
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
            }
        }


        private void DeleteRowFromDatabase(string name, string fathername, string cnic, string clas)
        {
            // Delete row from database based on the given Idsss
            string query = "DELETE FROM student_left WHERE name = @sales and fathername=@Category and cnic=@Amount and class=@Description";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@sales", name);
                command.Parameters.AddWithValue("@Category", fathername);
                command.Parameters.AddWithValue("@Amount", cnic);
                command.Parameters.AddWithValue("@Description", clas);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

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
                    // Delete row from database
                    DeleteRowFromDatabase(sname, fathername, cnic, sclass);

                    // Remove row from DataGridView
                    dataGridView1.Rows.Remove(row);
                }

                
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            DataTable dataTable = new DataTable();

            string searchText = search.Text;
            string selectedClass = comboBox1.SelectedItem?.ToString();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Base query
                string query = "SELECT  name, fathername, address, date_of_admission, contact_no, alternate_no, guardian_no, relation_with_guardian, previous_qualification, cnic, class FROM student_left WHERE 1=1";

                // Append conditions based on input
                if (!string.IsNullOrEmpty(searchText))
                {
                    query += " AND (cnic = @searchText OR name = @searchText)";
                }
                if (!string.IsNullOrEmpty(selectedClass))
                {
                    query += " AND class = @selectedClass";
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

        private void left_students_Load(object sender, EventArgs e)
        {
            
         comboboxshow();
            
        }
    }
}
