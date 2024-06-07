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
using System.Xml.Linq;
using static madarsa_student_app.dashboard;

namespace madarsa_student_app
{
    public partial class add_reshult : Form
    {
        private string ConnectionString = dashboard.ConnectionString;
        public add_reshult()
        {
            InitializeComponent();
            dataGridView1.CellClick += dataGridView1_CellClick;
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

       
        private void button1_Click(object sender, EventArgs e)
        {

            DataTable dataTable = new DataTable();

            string searchText = search.Text;
            string selectedClass = klas.SelectedItem?.ToString();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Base query with join, including class_id
                string query = @"
        SELECT 
            sa.name, 
            sa.fathername, 
            sa.cnic, 
            sa.class_id,  -- Include class_id
            c.class 
        FROM 
            student_admission sa
        INNER JOIN 
            class c ON sa.class_id = c.id
        WHERE 
            1=1";

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           

        }

        private void DisplayClassDetails(int classId, string studentName,string klass)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();

                    // Retrieve subjects for the selected class
                    string[] subjects = new string[7];
                    using (SqlCommand getSubjectsCmd = new SqlCommand("SELECT subject1, subject2, subject3, subject4, subject5, subject6, subject7 FROM class WHERE id = @classId", sqlConnection))
                    {
                        getSubjectsCmd.Parameters.AddWithValue("@classId", classId);
                        using (SqlDataReader reader = getSubjectsCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                for (int i = 0; i < 7; i++)
                                {
                                    subjects[i] = reader.IsDBNull(i) ? null : reader.GetString(i);
                                }
                            }
                            else
                            {
                                MessageBox.Show("No class found with the given id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    // Display subjects in labels
                    for (int i = 0; i < subjects.Length; i++)
                    {
                        Label subjectLabel = Controls.Find("subject" + (i + 1) + "Label", true).FirstOrDefault() as Label;
                        if (subjectLabel != null)
                        {
                            if (!string.IsNullOrEmpty(subjects[i]))
                            {
                                subjectLabel.Text = subjects[i];
                                subjectLabel.Visible = true;
                            }
                            else
                            {
                                subjectLabel.Visible = false;
                            }
                        }
                    }

                    // Find the name textbox control and set its text
                    TextBox nameTextBox = Controls.Find("search", true).FirstOrDefault() as TextBox;
                    if (nameTextBox != null)
                    {
                        nameTextBox.Text = studentName;
                    }

                    // Find the class combobox control and set its selected item
                    ComboBox classComboBox = Controls.Find("klas", true).FirstOrDefault() as ComboBox;
                    if (classComboBox != null)
                    {
                        classComboBox.SelectedItem = klass;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void add_reshult_Load(object sender, EventArgs e)
        {
            comboboxshow();
        }

        
           

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Assuming the class ID is in a column named "class_id"
                int classId = Convert.ToInt32(row.Cells["class_id"].Value);

                // Assuming the student's name is in a column named "name"
                string studentName = row.Cells["name"].Value.ToString();

                // Assuming the class name is in a column named "class"
                string klass = row.Cells["class"].Value.ToString();

                // Display class details and subjects for the selected class
                DisplayClassDetails(classId, studentName, klass);
            }
        }

        private void subject3label_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s1 = sub1.Text;
            string s2 = sub2.Text;
            string s3 = sub3.Text;
            string s4 = sub4.Text;
            string s5 = sub5.Text;
            string s6 = sub6.Text;
            string s7 = sub7.Text;
            string name = search.Text;
            string clas = klas.Text;
            string paper = comboBox2.SelectedItem?.ToString();

            // Check if any required field is empty
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(clas) || string.IsNullOrEmpty(paper))
            {
                MessageBox.Show("Please fill in the required fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();

                    // Retrieve student_id based on the student's name
                    int studentId;
                    using (SqlCommand getStudentIdCmd = new SqlCommand("SELECT id FROM student_admission WHERE name = @studentName", sqlConnection))
                    {
                        getStudentIdCmd.Parameters.AddWithValue("@studentName", name);
                        object result = getStudentIdCmd.ExecuteScalar();
                        if (result == null)
                        {
                            MessageBox.Show("Student not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        studentId = (int)result;
                    }

                    // Retrieve class_id based on the selected class name
                    int classId;
                    using (SqlCommand getClassIdCmd = new SqlCommand("SELECT id FROM class WHERE class = @className", sqlConnection))
                    {
                        getClassIdCmd.Parameters.AddWithValue("@className", clas);
                        object result = getClassIdCmd.ExecuteScalar();
                        if (result == null)
                        {
                            MessageBox.Show("Class not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        classId = (int)result;
                    }

                    // Insert data into result table
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO result (student_id, class_id, student_name, class_name, examname, s1marks, s2marks, s3marks, s4marks, s5marks, s6marks, s7marks) VALUES (@studentId, @classId, @name, @className, @examname, @s1marks, @s2marks, @s3marks, @s4marks, @s5marks, @s6marks, @s7marks)", sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@studentId", studentId);
                        cmd.Parameters.AddWithValue("@classId", classId);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@className", clas);
                        cmd.Parameters.AddWithValue("@examname", paper);
                        cmd.Parameters.AddWithValue("@s1marks", string.IsNullOrEmpty(s1) ? (object)DBNull.Value : Convert.ToInt32(s1));
                        cmd.Parameters.AddWithValue("@s2marks", string.IsNullOrEmpty(s2) ? (object)DBNull.Value : Convert.ToInt32(s2));
                        cmd.Parameters.AddWithValue("@s3marks", string.IsNullOrEmpty(s3) ? (object)DBNull.Value : Convert.ToInt32(s3));
                        cmd.Parameters.AddWithValue("@s4marks", string.IsNullOrEmpty(s4) ? (object)DBNull.Value : Convert.ToInt32(s4));
                        cmd.Parameters.AddWithValue("@s5marks", string.IsNullOrEmpty(s5) ? (object)DBNull.Value : Convert.ToInt32(s5));
                        cmd.Parameters.AddWithValue("@s6marks", string.IsNullOrEmpty(s6) ? (object)DBNull.Value : Convert.ToInt32(s6));
                        cmd.Parameters.AddWithValue("@s7marks", string.IsNullOrEmpty(s7) ? (object)DBNull.Value : Convert.ToInt32(s7));
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Result saved successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}
