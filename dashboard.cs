using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace madarsa_student_app
{
    public partial class dashboard : Form
    {
        public static string ConnectionString = "Data Source=WELA-MASROOF\\SQLEXPRESS;Initial Catalog=madarsastudent;Integrated Security=True;Encrypt=False";
        public dashboard()
        {
            InitializeComponent();
        }

        private void dashboard_Load(object sender, EventArgs e)
        {

        }
        public void loadform(object Form)
        {
            if (this.panel2.Controls.Count > 0)
                this.panel2.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.panel2.Controls.Add(f);
            this.panel2.Tag = f;
            f.Show();

        }

        private void studentdetails_Click(object sender, EventArgs e)
        {
            loadform(new studentdetails());
        }

        private void studentresult_Click(object sender, EventArgs e)
        {
            loadform(new add_reshult());
        }

        private void addstudent_Click_1(object sender, EventArgs e)
        {
            loadform(new addstudent());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadform(new left_students());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadform(new new_class());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            loadform(new showresult());
        }
    }
}
