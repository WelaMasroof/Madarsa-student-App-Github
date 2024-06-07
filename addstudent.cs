using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static madarsa_student_app.dashboard;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace madarsa_student_app
{
    public partial class addstudent : Form
    {
        private string ConnectionString = dashboard.ConnectionString;
        public addstudent()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void addstudent_Load(object sender, EventArgs e)
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
                                clasbox.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                }
                sqlConnection.Close();
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            string sname = name.Text;
            string fname = fathername.Text;
            string saddress = address.Text;
            string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string cellno = contact.Text;
            string alterno = alternativecontact.Text;
            string sgardianno = gardianno.Text;
            string rwithgardian = relationwithgardian.Text;
            string pstudy = previousstudy.Text;
            string CNIC = cnic.Text;
            string clas = clasbox.SelectedItem?.ToString();

            // Check if any required field is empty
            if (string.IsNullOrEmpty(sname) || string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(saddress) || string.IsNullOrEmpty(date) || string.IsNullOrEmpty(cellno) || string.IsNullOrEmpty(sgardianno) || string.IsNullOrEmpty(rwithgardian) || string.IsNullOrEmpty(pstudy) || string.IsNullOrEmpty(CNIC) || string.IsNullOrEmpty(clas))
            {
                MessageBox.Show("Please fill in the required fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();

                    // Retrieve class_id based on the selected class name
                    int classId;
                    using (SqlCommand getClassIdCmd = new SqlCommand("SELECT id FROM class WHERE class = @className", sqlConnection))
                    {
                        getClassIdCmd.Parameters.AddWithValue("@className", clas);
                        classId = (int)getClassIdCmd.ExecuteScalar();
                    }

                    // Insert data into student_admission table
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO student_admission (name, fathername, address, date_of_admission, contact_no, alternate_no, guardian_no, relation_with_guardian, previous_qualification, cnic, class_id) VALUES (@sname, @fname, @saddress, @date, @cellno, @alterno, @sgardianno, @rwithgardian, @pstudy, @CNIC, @classId)", sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@sname", sname);
                        cmd.Parameters.AddWithValue("@fname", fname);
                        cmd.Parameters.AddWithValue("@saddress", saddress);
                        cmd.Parameters.AddWithValue("@date", date);
                        cmd.Parameters.AddWithValue("@cellno", cellno);
                        cmd.Parameters.AddWithValue("@alterno", alterno);
                        cmd.Parameters.AddWithValue("@sgardianno", sgardianno);
                        cmd.Parameters.AddWithValue("@rwithgardian", rwithgardian);
                        cmd.Parameters.AddWithValue("@pstudy", pstudy);
                        cmd.Parameters.AddWithValue("@CNIC", CNIC);
                        cmd.Parameters.AddWithValue("@classId", classId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"{sname} Saved Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
