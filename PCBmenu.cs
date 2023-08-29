using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Downtime_registration
{
    public partial class PCBmenu : Form
    {
        public PCBmenu()
        {
            InitializeComponent();
        }
        public new void Dispose()
        {
            pictureBox10.Image.Dispose();
            pictureBox10.Image = null;
            pictureBox11.Image.Dispose();
            pictureBox11.Image = null;
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Dispose();
            PCBprodRelease nextForm = new PCBprodRelease();
            Hide();
            nextForm.ShowDialog();
            Close();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Dispose();
            PCBoverview nextForm = new PCBoverview();
            Hide();
            nextForm.ShowDialog();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
            _1 nextForm = new _1();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
    }
}
