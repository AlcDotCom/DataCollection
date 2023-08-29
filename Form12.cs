using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Downtime_registration.Properties;
using System.Configuration;

namespace Downtime_registration
{
    public partial class Form12 : Form
    {
        public Form12()
        {
            InitializeComponent();
            Settings.Default.nastavenieHV = "0";
            Settings.Default.nastavenieJM = "0";
            Settings.Default.nastavenieST = "0";
            Settings.Default.nastavenieMAT = "0";
            Settings.Default.nastaveniePROC = "0";
            Settings.Default.nastavenieNEK = "0";
            Settings.Default.nastavenieCENA = "0";
            Settings.Default.nastavenieCYKLUS = "0";
            Settings.Default.nastavenieOP = "0";
            Settings.Default.Save();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
            _1 nextForm = new _1();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Dispose();
            Form6 nextForm = new Form6();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Settings.Default.nastavenieHV = "1";
            Settings.Default.Save();
            Dispose();
            Form120 nextForm = new Form120();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Settings.Default.nastavenieJM = "1";
            Settings.Default.Save();
            Dispose();
            Form120 nextForm = new Form120();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Settings.Default.nastavenieST = "1";
            Settings.Default.Save();
            Dispose();
            Form120 nextForm = new Form120();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Settings.Default.nastaveniePROC = "1";
            Settings.Default.Save();
            Dispose();
            Form120 nextForm = new Form120();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Settings.Default.nastavenieNEK = "1";
            Settings.Default.Save();
            Dispose();
            Form120 nextForm = new Form120();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Settings.Default.nastavenieMAT = "1";
            Settings.Default.Save();
            Dispose();
            Form120 nextForm = new Form120();
            Hide();
            nextForm.ShowDialog();
            Close();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Settings.Default.nastavenieCENA = "1";
            Settings.Default.Save();
            Dispose();
            Form120 nextForm = new Form120();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Settings.Default.nastavenieCYKLUS = "1";
            Settings.Default.Save();
            Dispose();
            Form120 nextForm = new Form120();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Settings.Default.nastavenieOP = "1";
            Settings.Default.Save();
            Dispose();
            Form120 nextForm = new Form120();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        public new void Dispose()
        {
            pictureBox1.Image.Dispose();
            pictureBox1.Image = null;
            pictureBox2.Image.Dispose();
            pictureBox2.Image = null;
            pictureBox3.Image.Dispose();
            pictureBox3.Image = null;
            pictureBox4.Image.Dispose();
            pictureBox4.Image = null;
            pictureBox5.Image.Dispose();
            pictureBox5.Image = null;
            pictureBox6.Image.Dispose();
            pictureBox6.Image = null;
            pictureBox7.Image.Dispose();
            pictureBox7.Image = null;
            pictureBox8.Image.Dispose();
            pictureBox8.Image = null;
            pictureBox9.Image.Dispose();
            pictureBox9.Image = null;
            pictureBox10.Image.Dispose();
            pictureBox10.Image = null;
            pictureBox11.Image.Dispose();
            pictureBox11.Image = null;
        }
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Dispose();
            Form118 nextForm = new Form118();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
    }
}
