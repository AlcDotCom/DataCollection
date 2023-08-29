using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Downtime_registration.Properties;

namespace Downtime_registration 
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Text = "Zápis prestoja - výber kategórie na zariadení " + Settings.Default.NazovZariadenia;
            if (Settings.Default.NazovZariadenia == "LHINTEN")  //prispôsobenie pre LHINTEN zariadenie
            { WindowState = FormWindowState.Normal; }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dispose();
            Form2 nextForm = new Form2();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Dispose();
            Form3 nextForm = new Form3();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Dispose();
            Form7 nextForm = new Form7();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Dispose();
            Form8 nextForm = new Form8();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Dispose();
            Form9 nextForm = new Form9();
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
              tableLayoutPanel1.Dispose();
              tableLayoutPanel1 = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
            _1 nextForm = new _1();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
    }
}
