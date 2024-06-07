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
    public partial class new_class : Form
    {
        private string ConnectionString = dashboard.ConnectionString;

        public new_class()
        {
            InitializeComponent();
        }

        private void save_Click(object sender, EventArgs e)
        {
            string klass = nameclass.Text;
            string su1 = s1.Text;
            string su2 = s2.Text;
            string su3 = s3.Text;
            string su4 = s4.Text;
            string su5 = s5.Text;
            string su6 = s6.Text;
            string su7 = s7.Text;
            

            if (string.IsNullOrEmpty(su1) || string.IsNullOrEmpty(klass))
            {
                MessageBox.Show("Please fill in the required fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO class (class, subject1, subject2, subject3, subject4, subject5, subject6, subject7) VALUES (@klass, @su1, @su2, @su3, @su4, @su5, @su6, @su7)", sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@klass", klass);
                        cmd.Parameters.AddWithValue("@su1", su1);
                        cmd.Parameters.AddWithValue("@su2", su2);
                        cmd.Parameters.AddWithValue("@su3", su3);
                        cmd.Parameters.AddWithValue("@su4", su4);
                        cmd.Parameters.AddWithValue("@su5", su5);
                        cmd.Parameters.AddWithValue("@su6", su6);
                        cmd.Parameters.AddWithValue("@su7", su7);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show(klass + " Saved Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
