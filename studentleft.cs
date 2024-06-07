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
using System.Xml.Linq;

namespace madarsa_student_app
{
    public partial class studentleft : Form
    {
        
        public studentleft()
        {
            InitializeComponent();
        }

        private void studentleft_Load(object sender, EventArgs e)
        {

        }
        private string ConnectionString = dashboard.ConnectionString;
        string sname = studentdetails.sname;
        string fathername = studentdetails.fathername;
        string address = studentdetails.address;
        string date = studentdetails.date;
        string contact = studentdetails.contact;
        string alterno = studentdetails.alterno;
        string gardianno = studentdetails.gardianno;
        string relationwithgardian = studentdetails.relationwithgardian;
        string previousqualification = studentdetails.previousqualification;
        string sclass = studentdetails.sclass;
        string cnic = studentdetails.cnic;
        private void add_Click(object sender, EventArgs e)
        {

            string Reason = reason.Text;
            string Tenure = studytenure.Text;
            string Dates = dateTimePicker1.Value.ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(Reason) || string.IsNullOrEmpty(Tenure))
            {
                MessageBox.Show("Please fill in the required fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO student_left (name, fathername, address, date_of_admission, contact_no, alternate_no, guardian_no, relation_with_guardian, previous_qualification, cnic, class,duration_of_study,reason_to_leave,date_of_left) VALUES (@sname, @fname, @saddress, @date, @cellno, @alterno, @sgardianno, @rwithgardian, @pstudy, @CNIC,@clas,@Reason,@Tenure,@Dates)", sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@sname", sname);
                        cmd.Parameters.AddWithValue("@fname", fathername);
                        cmd.Parameters.AddWithValue("@saddress", address);
                        cmd.Parameters.AddWithValue("@date", date);
                        cmd.Parameters.AddWithValue("@cellno", contact);
                        cmd.Parameters.AddWithValue("@alterno", alterno);
                        cmd.Parameters.AddWithValue("@sgardianno", gardianno);
                        cmd.Parameters.AddWithValue("@rwithgardian", relationwithgardian);
                        cmd.Parameters.AddWithValue("@pstudy", previousqualification);
                        cmd.Parameters.AddWithValue("@CNIC", cnic);
                        cmd.Parameters.AddWithValue("@clas", sclass);
                        cmd.Parameters.AddWithValue("@Reason", Reason);
                        cmd.Parameters.AddWithValue("@Tenure", Tenure);
                        cmd.Parameters.AddWithValue("@Dates", Dates);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show(sname + " Saved Successfully");
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
